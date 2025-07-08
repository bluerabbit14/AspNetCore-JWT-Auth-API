using Asp_.Net_Web_Api.Model.Domain;

namespace Asp_.Net_Web_Api.Interface
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(int id);
        Task UpdateProfileAsync(User user);
    }
}
