using Asp_.Net_Web_Api.Data;
using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.Domain;
using Asp_.Net_Web_Api.Model.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Asp_.Net_Web_Api.Services
{
    public class RegisterService: IRegisterService
    {
        private readonly UserDbContext _context;
        public RegisterService(UserDbContext context)
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
                Message = "User Registered Successfully",
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
    }
}
