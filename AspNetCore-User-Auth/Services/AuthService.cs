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
        private readonly IJwtService _jwtService;
        public AuthService(UserDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
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

            if(user.IsActive == false)
                throw new InvalidOperationException("User is not active");

            var userPassword = await _context.UserPasswords.FirstOrDefaultAsync(p => p.UserId == user.UserId);
            if (userPassword == null || string.IsNullOrEmpty(userPassword.Salt))
                throw new InvalidOperationException("User does not exist!");

            var hashedPassword = PasswordHelper.HashPassword(loginRequestDto.Password, userPassword.Salt);

            if (hashedPassword != userPassword.PasswordHash)
                throw new InvalidOperationException("Invalid Password");

            user.RememberMe = loginRequestDto.RememberMe;
            user.LastLogin = loginRequestDto.LastLogin;
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);
            var tokenExpiry = _jwtService.GetTokenExpiration(token);
            var result = new LoginResponseDTO()
            {
                Message = "User Logged in successfully",
                UserId = user.UserId,
                Token = token,
                TokenValidity = (DateTime)tokenExpiry
            };
            return result;
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
            return !string.IsNullOrEmpty(credential) && credential.Contains("@") && credential.Contains(".");
        }
    }
}