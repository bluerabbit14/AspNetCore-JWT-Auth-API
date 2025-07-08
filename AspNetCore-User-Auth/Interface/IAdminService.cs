using Asp_.Net_Web_Api.Model.Domain;
using Asp_.Net_Web_Api.Model.DTO;

namespace Asp_.Net_Web_Api.Interface
{
    public interface IAdminService
    {
        Task<List<UserProfile>> GetAllUserProfilesAsync(); 
        Task<List<UserProfile>> GetUserProfileByRoleAsync(string role, int? userId = null); 
        Task<UserProfile> GetUserProfileAsync(int id); 
        Task<UserProfile> UpdateUserProfileByAdminAsync(int id, UpdateProfileByAdminDTO userProfileDto);
        Task<object> AlterUserActiveStatus(int id, bool status);
        Task<object> DeleteUserProfileAsync(int id);
        Task<UserProfile> GetUserProfileByEmailAsync(string email);
        Task<UserProfile> GetUserProfileByPhoneAsync(string phone);
        Task<List<UserProfile>> GetUserProfileByGenderAsync(string gender);
    }
}
