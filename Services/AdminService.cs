using Asp_.Net_Web_Api.Data;
using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.Domain;
using Asp_.Net_Web_Api.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace Asp_.Net_Web_Api.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserDbContext _context;
        public AdminService(UserDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserProfile>> GetAllUserProfilesAsync()
        {
            var profiles = await _context.UserProfilies.ToListAsync();
            if (profiles == null)
                throw new KeyNotFoundException($"No Data found");
            return profiles;
        }
        public async Task<List<UserProfile>> GetUserProfileByRoleAsync(string role, int? userId = null)
        {
            if (string.IsNullOrWhiteSpace(role))
                return new List<UserProfile>();

            var roleLower = role.ToLower();

            if (userId.HasValue)
            {
                // If id is passed, return the user with that id and role
                var userProfile = await _context.UserProfilies
                    .Where(u => u.UserId == userId.Value && (u.Role != null && u.Role.ToLower() == roleLower))
                    .ToListAsync();
                return userProfile;
            }
            else
            {
                // If id is not passed, return all users with the given role
                var profiles = await _context.UserProfilies
                    .Where(u => u.Role != null && u.Role.ToLower() == roleLower)
                    .ToListAsync();
                return profiles;
            }
        }
        public async Task<UserProfile> GetUserProfileAsync(int id)
        {
            var user = await _context.UserProfilies.FindAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");
            return user;
        }
        public async Task<object> AlterUserActiveStatus(int id, bool status)
        {
            var user = _context.UserProfilies.Find(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");
            user.IsActive = status;
            user.LastUpdated = DateTime.UtcNow;
            _context.UserProfilies.Update(user);
            _context.SaveChanges();
            return new { Message = $"User with ID {id} is now {(status ? "active" : "inactive")}." };  
        }
        public async Task<UserProfile> UpdateUserProfileByAdminAsync(int id, UpdateProfileByAdminDTO userProfileDto)
        {
            var user = await _context.UserProfilies.FindAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            var originalUser = JsonSerializer.Serialize(user);

            if (userProfileDto.ImageUrl != null)
                user.ImageUrl = userProfileDto.ImageUrl;

            if (userProfileDto.Name != null)
                user.Name = userProfileDto.Name;

            if (userProfileDto.Email != null)
                user.Email = userProfileDto.Email;

            if (userProfileDto.Phone != null)
                user.Phone = userProfileDto.Phone;

            if (userProfileDto.Address != null)
                user.Address = userProfileDto.Address;

            if (userProfileDto.Pincode != null)
                user.Pincode = userProfileDto.Pincode;

            if (userProfileDto.DateOfBirth != null)
                user.DateOfBirth = userProfileDto.DateOfBirth;

            if (userProfileDto.Gender != null)
                user.Gender = userProfileDto.Gender;

            if (userProfileDto.Role != null)
                user.Role = userProfileDto.Role;

            if (userProfileDto.LanguagePreference != null)
                user.LanguagePreference = userProfileDto.LanguagePreference;

            if (userProfileDto.Timezone != null)
                user.Timezone = userProfileDto.Timezone;

            if (userProfileDto.Bio != null)
                user.Bio = userProfileDto.Bio;

            if (userProfileDto.SocialHandle != null)
                user.SocialHandle = userProfileDto.SocialHandle;

            if (userProfileDto.IsEmailVerified.HasValue)
                user.IsEmailVerified = userProfileDto.IsEmailVerified.Value;

            if (userProfileDto.IsPhoneVerified.HasValue)
                user.IsPhoneVerified = userProfileDto.IsPhoneVerified.Value;

            if (userProfileDto.IsActive.HasValue)
                user.IsActive = userProfileDto.IsActive.Value;

            if (userProfileDto.RememberMe.HasValue)
                user.RememberMe = userProfileDto.RememberMe.Value;

            user.LastUpdated = DateTime.UtcNow;

            var updatedUser = JsonSerializer.Serialize(user);
            if (originalUser != updatedUser)
            {
                _context.UserProfilies.Update(user);
                await _context.SaveChangesAsync();
            }

            return user;
        }
        public async Task<object> DeleteUserProfileAsync(int id)
        {
            var user = await _context.UserProfilies.FindAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");
            _context.UserProfilies.Remove(user);
            await _context.SaveChangesAsync();

            return new { Message = $"User with ID {id} has been deleted." };
        }
        public async Task<UserProfile> GetUserProfileByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            var user = await _context.UserProfilies.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new KeyNotFoundException($"User with email {email} not found.");
            return user;
        }
        public async Task<UserProfile> GetUserProfileByPhoneAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Phone cannot be null or empty.", nameof(phone));
            var user = await _context.UserProfilies.FirstOrDefaultAsync(u => u.Phone == phone);
            if (user == null)
                throw new KeyNotFoundException($"User with phone {phone} not found.");
            return user;
        }
        public async Task<List<UserProfile>> GetUserProfileByGenderAsync(string gender)
        {
            if (string.IsNullOrWhiteSpace(gender))
                throw new ArgumentException("Gender cannot be null or empty.", nameof(gender));

            // Corrected the usage of ToListAsync by using Where before calling ToListAsync  
            var users = await _context.UserProfilies
                .Where(u => u.Gender == gender)
                .ToListAsync();

            if (users == null || !users.Any())
                throw new KeyNotFoundException($"No users found with Gender {gender}.");

            return users;
        }
    }
}
