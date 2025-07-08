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
            if(profiles==null)
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
    }
}
