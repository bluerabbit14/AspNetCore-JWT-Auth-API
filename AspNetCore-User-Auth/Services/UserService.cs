using Asp_.Net_Web_Api.Data;
using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.Domain;
using Asp_.Net_Web_Api.Model.DTO;
using System.Security.Claims;
using System.Text.Json;

namespace Asp_.Net_Web_Api.Services
{
    public class UserService:IUserService
    {
        private readonly UserDbContext _context;
        public UserService(UserDbContext context)
        {
            _context = context;
        }
        public async Task<GetUserDTO> GetUserProfileAsync(int id)
        {

            var user = await _context.UserProfilies.FindAsync(id);
            if (user == null)
             throw new KeyNotFoundException($"User with ID {id} not found.");

            var result = new GetUserDTO
            {
                UserId = user.UserId,
                ImageUrl = user.ImageUrl,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                Pincode = user.Pincode,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                LastUpdated = user.LastUpdated,
                LanguagePreference = user.LanguagePreference,
                Timezone = user.Timezone,
                IsEmailVerified = user.IsEmailVerified,
                IsPhoneVerified = user.IsPhoneVerified,
                Bio = user.Bio,
                SocialHandle = user.SocialHandle
            };
            return result;
        }
        public async Task<UpdateProfileByUserDTO> UpdateUserProfileAsync(int id, UpdateProfileByUserDTO userProfileDto)
        {
            var user = await _context.UserProfilies.FindAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            user.ImageUrl = userProfileDto.ImageUrl;
            user.Name = userProfileDto.Name;
            user.Address = userProfileDto.Address;
            user.Pincode = userProfileDto.Pincode;
            user.DateOfBirth = userProfileDto.DateOfBirth;
            user.Gender = userProfileDto.Gender;
            user.LanguagePreference = userProfileDto.LanguagePreference;
            user.Bio = userProfileDto.Bio;
            user.SocialHandle = userProfileDto.SocialHandle;
            user.LastUpdated = DateTime.UtcNow;

            _context.UserProfilies.Update(user);
            await _context.SaveChangesAsync();

            var result = new UpdateProfileByUserDTO
            {
               ImageUrl= user.ImageUrl,
               Name = user.Name,
               Address = user.Address,
               Pincode = user.Pincode ,
               DateOfBirth = user.DateOfBirth,
               Gender = user.Gender,
               LanguagePreference = user.LanguagePreference,
               Bio =user.Bio,
               SocialHandle = user.SocialHandle
            };
            return result;
        }
        public async Task<UserProfile> PatchUserProfileAsync(int id, UpdateProfileByUserDTO userProfileDto)
        {
            var user = await _context.UserProfilies.FindAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            var originalUser = JsonSerializer.Serialize(user);

            if (userProfileDto.ImageUrl != null)
                user.ImageUrl = userProfileDto.ImageUrl;

            if (userProfileDto.Name != null)
                user.Name = userProfileDto.Name;

            if (userProfileDto.Address != null)
                user.Address = userProfileDto.Address;

            if (userProfileDto.Pincode != null)
                user.Pincode = userProfileDto.Pincode;

            if (userProfileDto.DateOfBirth != null)
                user.DateOfBirth = userProfileDto.DateOfBirth;

            if (userProfileDto.Gender != null)
                user.Gender = userProfileDto.Gender;

            if (userProfileDto.LanguagePreference != null)
                user.LanguagePreference = userProfileDto.LanguagePreference;


            if (userProfileDto.Bio != null)
                user.Bio = userProfileDto.Bio;

            if (userProfileDto.SocialHandle != null)
                user.SocialHandle = userProfileDto.SocialHandle;


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
