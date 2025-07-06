using Asp_.Net_Web_Api.Data;
using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.Domain;
using Asp_.Net_Web_Api.Model.DTO;
using Asp_.Net_Web_Api.Utility;
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
        public async Task<Result<SignUpResponseDTO>> RegisterUserAsync(SignUpRequestDTO signUpRequestDto)
        {

            if (string.IsNullOrWhiteSpace(signUpRequestDto.Email))
                return Result<SignUpResponseDTO>.Failure("email is required.");

            if (string.IsNullOrWhiteSpace(signUpRequestDto.Phone))
                return Result<SignUpResponseDTO>.Failure("phone is required.");

            if (string.IsNullOrWhiteSpace(signUpRequestDto.Password))
                return Result<SignUpResponseDTO>.Failure("Password is required.");

            var existingUser = await _context.UserProfilies.FirstOrDefaultAsync(u =>
                (!string.IsNullOrEmpty(signUpRequestDto.Email) && u.Email == signUpRequestDto.Email) ||
                (!string.IsNullOrEmpty(signUpRequestDto.Phone) && u.Phone == signUpRequestDto.Phone));

            if (existingUser != null)
            {
                var conflictMessage = (!string.IsNullOrEmpty(signUpRequestDto.Email) && existingUser.Email == signUpRequestDto.Email)
                                    ? "Email is already registered."
                                    : "Phone is already registered.";
                return Result<SignUpResponseDTO>.Failure(conflictMessage);
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
                UserId = newUser.UserId,
                Email = newUser.Email,
                CreatedAt = newUser.CreatedAt,
            };
            return Result<SignUpResponseDTO>.Success(result,"User Registered Successfully");
        }
        public async Task<Result<LoginResponseDTO>> LoginUserAsync(LoginRequestDTO loginRequestDto)
        {
            if (string.IsNullOrWhiteSpace(loginRequestDto.Credentials) || string.IsNullOrWhiteSpace(loginRequestDto.Password))
                return Result<LoginResponseDTO>.Failure("Email/Phone and Password are required.");

            var user = await _context.UserProfilies
                .FirstOrDefaultAsync(u => u.Email == loginRequestDto.Credentials || u.Phone == loginRequestDto.Credentials);

            if (user == null)
                return Result<LoginResponseDTO>.Failure("User does not exist");

            var userPassword = await _context.UserPasswords.FirstOrDefaultAsync(p => p.UserId == user.UserId);
            if (userPassword == null || string.IsNullOrEmpty(userPassword.Salt))
                return Result<LoginResponseDTO>.Failure("User does not exist!");

            var hashedPassword = PasswordHelper.HashPassword(loginRequestDto.Password, userPassword.Salt);

            if (hashedPassword != userPassword.PasswordHash)
                return Result<LoginResponseDTO>.Failure("Invalid Password");

            user.RememberMe = loginRequestDto.RememberMe;
            user.LastLogin = loginRequestDto.LastLogin;
            await _context.SaveChangesAsync();


            var result = new LoginResponseDTO()
            { 
                   UserId=user.UserId
            };
            var token =GenerateToken(user);
            return Result<LoginResponseDTO>.Success("User Login Successfully", token, result);
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
    
        public async Task<Result<CredentialDTO>> UserExistAsync(CredentialDTO credentialDto)
        {
            if (string.IsNullOrWhiteSpace(credentialDto.Credential))
                return Result<CredentialDTO>.Failure("Credential is required.");
            var user = await _context.UserProfilies
                .FirstOrDefaultAsync(u => u.Email == credentialDto.Credential || u.Phone == credentialDto.Credential);
            if (user == null)
                return Result<CredentialDTO>.Failure("User not found");
            return Result<CredentialDTO>.Success(new()
            {
                Credential = user.Email ?? user.Phone,
                Name = user.Name,
                UserId = user.UserId
            },"User Exist");
        }
    }
}
