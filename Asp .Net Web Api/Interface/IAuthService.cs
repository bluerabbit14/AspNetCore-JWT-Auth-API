using Asp_.Net_Web_Api.Model.DTO;
using Asp_.Net_Web_Api.Utility;

namespace Asp_.Net_Web_Api.Interface
{
    public interface IAuthService 
    {
        Task<Result<SignUpResponseDTO>> RegisterUserAsync(SignUpRequestDTO sigUpRequestDto);
        Task<Result<LoginResponseDTO>> LoginUserAsync(LoginRequestDTO loginRequestDto);
        Task<Result<CredentialDTO>> UserExistAsync(CredentialDTO credentialDto);
    }
}
