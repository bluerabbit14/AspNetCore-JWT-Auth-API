using Asp_.Net_Web_Api.Model.DTO;
using Asp_.Net_Web_Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Asp_.Net_Web_Api.Interface
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginUserAsync(LoginRequestDTO loginRequestDto);
        Task<UserExistResponseDTO> UserExistAsync(UserExistRequestDTO credentialDto);

    }
}
