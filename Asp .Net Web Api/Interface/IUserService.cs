using Asp_.Net_Web_Api.Model.Domain;
using Asp_.Net_Web_Api.Model.DTO;

namespace Asp_.Net_Web_Api.Interface
{
    public interface IUserService 
    {
        Task<UserProfile> UpdateUserProfileAsync(int id, UpdateProfileDTO userProfileDto);
        Task<bool> DeleteUserAsync(int id);
        Task<UserProfile?> GetUserProfileAsync(int id);
    }
}
