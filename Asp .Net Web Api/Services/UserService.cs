using Asp_.Net_Web_Api.Data;
using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.Domain;
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
        public async Task<UserProfile> UpdateUserProfileAsync(int id, UpdateProfileDTO userProfileDto)
        {
            var user = await _context.UserProfilies.FindAsync(id);
            if (user == null)
                return null;

            user.ImageUrl = userProfileDto.ImageUrl;
            user.Name = userProfileDto.Name;
           // user.Email = userProfileDto.Email;
           // user.Phone = userProfileDto.Phone;
            user.Address = userProfileDto.Address;
            user.Pincode = userProfileDto.Pincode;
            user.DateOfBirth = userProfileDto.DateOfBirth;
            user.Gender = userProfileDto.Gender;
            user.Role = userProfileDto.Role;
            user.LanguagePreference = userProfileDto.LanguagePreference;
            user.Bio = userProfileDto.Bio;
            user.SocialHandle = userProfileDto.SocialHandle;

            _context.UserProfilies.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.UserProfilies.FindAsync(id);
            if (user == null)
            {
                return false; 
            }
            _context.UserProfilies.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<UserProfile> GetUserProfileAsync(int id)
        {
            var user = await _context.UserProfilies.FindAsync(id);
            if(user == null)
            {
                return null; 
            }
            return await _context.UserProfilies.FindAsync(id);
        }
    }
}
