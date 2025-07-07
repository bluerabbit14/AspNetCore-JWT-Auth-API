using Asp_.Net_Web_Api.Data;
using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.DTO;

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
        public async Task<DeleteResponseDTO> DeleteUserAsync(int id)
        {
            var user = await _context.UserProfilies.FindAsync(id);
            if (user == null)
               throw new KeyNotFoundException($"User with ID {id} not found.");

            var createdAt = user.CreatedAt;
            _context.UserProfilies.Remove(user);
            await _context.SaveChangesAsync();

            var result = new DeleteResponseDTO
            {
                Message = $"User with ID {id} deleted successfully.",
                CreatedAt = createdAt
            };
            return result;
        }
        
    }
}
