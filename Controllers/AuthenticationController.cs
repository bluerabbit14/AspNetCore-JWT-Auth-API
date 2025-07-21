using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUserAsync(LoginRequestDTO loginRequestDto)
        {
            var response = await _authService.LoginUserAsync(loginRequestDto);
            if (response == null)
                return NotFound(response);
            return Ok(response);
        }

        [HttpPost("Credential_Exist")]
        public async Task<IActionResult> UserExistAsync(UserExistRequestDTO credentialDto)
        {
            var response = await _authService.UserExistAsync(credentialDto);
            if (response == null)
                return NotFound(response);
            return Ok(response);
        }
        
    }
}
