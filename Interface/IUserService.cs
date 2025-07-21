using Asp_.Net_Web_Api.Model.Domain;
using Asp_.Net_Web_Api.Model.DTO;

namespace Asp_.Net_Web_Api.Interface
{
    public interface IUserService 
    {
        Task<GetUserDTO> GetUserProfileAsync(int id); 
        Task<UpdateProfileByUserDTO> UpdateUserProfileAsync(int id, UpdateProfileByUserDTO userProfileDto);
        Task<UserProfile> PatchUserProfileAsync( int id, UpdateProfileByUserDTO userProfileDto);
    }
}
