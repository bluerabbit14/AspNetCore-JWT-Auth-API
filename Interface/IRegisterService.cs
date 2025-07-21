using Asp_.Net_Web_Api.Model.DTO;

namespace Asp_.Net_Web_Api.Interface
{
    public interface IRegisterService
    {
        Task<SignUpResponseDTO> RegisterUserAsync(SignUpRequestDTO sigUpRequestDto);
        Task<SignUpResponseDTO> RegisterAdminAsync(SignUpRequestDTO sigUpRequestDto);
    }
}
