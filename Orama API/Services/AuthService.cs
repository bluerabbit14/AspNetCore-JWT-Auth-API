using Asp_.Net_Web_Api.Data;
using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.Domain;
using Asp_.Net_Web_Api.Model.DTO;
using Asp_.Net_Web_Api.Utility;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Asp_.Net_Web_Api.Services
{
    public class AuthService: IAuthService
    {
        private readonly UserDbContext _context;

        public AuthService(UserDbContext context)
        {
            _context = context;
        }
        public async Task<SignUpResponseDTO> RegisterUserAsync(SignUpRequestDTO signUpRequestDto)
        {
            if (string.IsNullOrWhiteSpace(signUpRequestDto.Name))
                throw new ArgumentException("Name is required.");

            if (string.IsNullOrWhiteSpace(signUpRequestDto.Email))
                throw new ArgumentException("Email is required.");

            if (string.IsNullOrWhiteSpace(signUpRequestDto.Phone))
                throw new ArgumentException("Phone is required.");

            if (string.IsNullOrWhiteSpace(signUpRequestDto.Password))
                throw new ArgumentException("Password is required.");

            if (string.IsNullOrWhiteSpace(signUpRequestDto.Gender))
                throw new ArgumentException("Gender is required.");

            if (string.IsNullOrWhiteSpace(signUpRequestDto.DateOfBirth))
                throw new ArgumentException("Date of birth is required.");

            var existingUser = await _context.UserProfilies.FirstOrDefaultAsync(u =>
                (!string.IsNullOrEmpty(signUpRequestDto.Email) && u.Email == signUpRequestDto.Email) ||
                (!string.IsNullOrEmpty(signUpRequestDto.Phone) && u.Phone == signUpRequestDto.Phone));

            if (existingUser != null)
            {
                var conflictMessage = (!string.IsNullOrEmpty(signUpRequestDto.Email) && existingUser.Email == signUpRequestDto.Email)
                                    ? "Email is already registered."
                                    : "Phone is already registered.";
                throw new InvalidOperationException(conflictMessage);
            }
            var newUser = new UserProfile
            {
                Name = signUpRequestDto.Name,
                Email = signUpRequestDto.Email,
                Phone = signUpRequestDto.Phone,
                Gender = signUpRequestDto.Gender,
                DateOfBirth = signUpRequestDto.DateOfBirth,
                CreatedAt = DateTime.UtcNow,
                RememberMe = false,
                IsActive = true
            };

            await _context.UserProfilies.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var salt = PasswordHelper.GenerateSalt();
            var hashedPassword = PasswordHelper.HashPassword(signUpRequestDto.Password, salt);

            var newUserPassword = new UserPassword
            {
                UserId = newUser.UserId,
                PasswordHash = hashedPassword,
                Salt = salt,
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow
            };

            await _context.UserPasswords.AddAsync(newUserPassword);
            await _context.SaveChangesAsync();
            var result = new SignUpResponseDTO
            {
                Message="User Registered Successfully",
                UserId = newUser.UserId,
                Email = newUser.Email ?? string.Empty,
                CreatedAt = newUser.CreatedAt,
            };
            return result;
        }
        public async Task<SignUpResponseDTO> RegisterAdminAsync(SignUpRequestDTO signUpRequestDto)
        {
            if (string.IsNullOrWhiteSpace(signUpRequestDto.Name))
                throw new ArgumentException("Name is required.");

            if (string.IsNullOrWhiteSpace(signUpRequestDto.Email))
                throw new ArgumentException("Email is required.");

            if (string.IsNullOrWhiteSpace(signUpRequestDto.Phone))
                throw new ArgumentException("Phone is required.");

            if (string.IsNullOrWhiteSpace(signUpRequestDto.Password))
                throw new ArgumentException("Password is required.");

            if (string.IsNullOrWhiteSpace(signUpRequestDto.Gender))
                throw new ArgumentException("Gender is required.");

            if (string.IsNullOrWhiteSpace(signUpRequestDto.DateOfBirth))
                throw new ArgumentException("Date of birth is required.");

            var existingUser = await _context.UserProfilies.FirstOrDefaultAsync(u =>
                (!string.IsNullOrEmpty(signUpRequestDto.Email) && u.Email == signUpRequestDto.Email) ||
                (!string.IsNullOrEmpty(signUpRequestDto.Phone) && u.Phone == signUpRequestDto.Phone));

            if (existingUser != null)
            {
                var conflictMessage = (!string.IsNullOrEmpty(signUpRequestDto.Email) && existingUser.Email == signUpRequestDto.Email)
                                    ? "Email is already registered."
                                    : "Phone is already registered.";
                throw new InvalidOperationException(conflictMessage);
            }
            var newUser = new UserProfile
            {
                Name = signUpRequestDto.Name,
                Email = signUpRequestDto.Email,
                Phone = signUpRequestDto.Phone,
                Gender = signUpRequestDto.Gender,
                DateOfBirth = signUpRequestDto.DateOfBirth,
                Role = "admin",
                CreatedAt = DateTime.UtcNow,
                RememberMe = false,
                IsActive = true
            };
            await _context.UserProfilies.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var salt = PasswordHelper.GenerateSalt();
            var hashedPassword = PasswordHelper.HashPassword(signUpRequestDto.Password, salt);

            var newUserPassword = new UserPassword
            {
                UserId = newUser.UserId,
                PasswordHash = hashedPassword,
                Salt = salt,
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow
            };

            await _context.UserPasswords.AddAsync(newUserPassword);
            await _context.SaveChangesAsync();
            var result = new SignUpResponseDTO
            {
                Message = "User Registered as 'Admin' Successfully",
                UserId = newUser.UserId,
                Email = newUser.Email ?? string.Empty,
                CreatedAt = newUser.CreatedAt,
            };
            return result;
        }
        public async Task<LoginResponseDTO> LoginUserAsync(LoginRequestDTO loginRequestDto)
        {
            if (string.IsNullOrWhiteSpace(loginRequestDto.Credential))
                throw new ArgumentException("Credential is  required field.");

            if (string.IsNullOrWhiteSpace(loginRequestDto.Password))
                throw new ArgumentException("Password is required field.");

            var user = await _context.UserProfilies
                .FirstOrDefaultAsync(u => u.Email == loginRequestDto.Credential || u.Phone == loginRequestDto.Credential);

            if (user == null)
                throw new InvalidOperationException("User does not exist");

            var userPassword = await _context.UserPasswords.FirstOrDefaultAsync(p => p.UserId == user.UserId);
            if (userPassword == null || string.IsNullOrEmpty(userPassword.Salt))
                throw new InvalidOperationException("User does not exist!");

            var hashedPassword = PasswordHelper.HashPassword(loginRequestDto.Password, userPassword.Salt);

            if (hashedPassword != userPassword.PasswordHash)
                throw new InvalidOperationException("Invalid Password");

            user.RememberMe = loginRequestDto.RememberMe;
            user.LastLogin = loginRequestDto.LastLogin;
            await _context.SaveChangesAsync();

            var token = GenerateToken(user);
            var tokenExpiry = GetTokenExpiration(token);
            var result = new LoginResponseDTO()
            {
                Message = "User Logged in successfully",
                UserId = user.UserId,
                Token = token,
                TokenValidity = tokenExpiry ?? DateTime.UtcNow.AddMinutes(JwtSetting.ExpirationMinutes)
            };
            return result;
        }
        public string GenerateToken(UserProfile user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.Name ?? ""),
                new Claim(ClaimTypes.Role, "User")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSetting.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescription = new JwtSecurityToken(
                issuer: JwtSetting.Issuer,
                audience: JwtSetting.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(JwtSetting.ExpirationMinutes),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescription);
        }
        public DateTime? GetTokenExpiration(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
                return null;

            var jwtToken = handler.ReadJwtToken(token);
            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);

            if (expClaim != null && long.TryParse(expClaim.Value, out long expUnix))
            {
                // Convert Unix time to UTC DateTime
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                return expirationTime;
            }
            return null;
        }
        public async Task<UserExistResponseDTO> UserExistAsync(UserExistRequestDTO credentialDto)
        {
            if (string.IsNullOrWhiteSpace(credentialDto.Credential))
                throw new ArgumentException("Credential is required.");

            var user = await _context.UserProfilies.FirstOrDefaultAsync(u => u.Email == credentialDto.Credential || u.Phone == credentialDto.Credential);
            if (user == null)
                throw new InvalidOperationException("User not found");

            // Determine if the credential is an email or phone number
            bool isEmail = IsValidEmail(credentialDto.Credential);
            bool isCredentialVerified = false;

            if (isEmail)
                isCredentialVerified = user.IsEmailVerified;
            else
                isCredentialVerified = user.IsPhoneVerified;

            var result = new UserExistResponseDTO
            {   
                Name = user.Name ?? string.Empty,
                Role = user.Role ?? "user",
                IsCredentialVerified = isCredentialVerified,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };
            return result;
        }
        private bool IsValidEmail(string credential)
        {
            // Simple email validation - checks for @ symbol and basic email format
            return !string.IsNullOrEmpty(credential) && credential.Contains("@") && credential.Contains(".");
        }
    }
}