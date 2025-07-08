using Asp_.Net_Web_Api.Model.Domain;
using Asp_.Net_Web_Api.Model.DTO;

namespace Asp_.Net_Web_Api.Interface
{
    public interface IAdminService
    {
        Task<UserProfile> UpdateUserProfileByAdminAsync(int id, UpdateProfileByAdminDTO userProfileDto);
    }
}
