using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Asp_.Net_Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController: ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUserAsync(SignUpRequestDTO signUpRequestDto)
        {
            var response = await _authService.RegisterUserAsync(signUpRequestDto);
            if (response == null)
                return NotFound(response);
            return Ok(response);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUserAsync(LoginRequestDTO loginRequestDto)
        {
            var response = await _authService.LoginUserAsync(loginRequestDto);
            if (response == null)
                return NotFound(response);
            return Ok(response);
        }
        [HttpPost("CredentialExist")]
        public async Task<IActionResult> UserExistAsync(CredentialDTO credentialDto)
        {
            var response = await _authService.UserExistAsync(credentialDto);
            if (response == null)
                return NotFound(response);
            return Ok(response);
        }
    }
}
