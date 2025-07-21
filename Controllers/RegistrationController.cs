using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.DTO;
using Asp_.Net_Web_Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Asp_.Net_Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController: ControllerBase
    {
        private readonly IRegisterService _registerService;
        public RegistrationController(IRegisterService registerService)
        {
            _registerService = registerService;
        }
        [HttpPost("User")]
        public async Task<IActionResult> RegisterUserAsync(SignUpRequestDTO signUpRequestDto)
        {
            var response = await _registerService.RegisterUserAsync(signUpRequestDto);
            if (response == null)
                return NotFound(response);
            return Ok(response);
        }


        [HttpPost("Admin")]
        public async Task<IActionResult> RegisterAdminAsync(SignUpRequestDTO signUpRequestDto)
        {
            var response = await _registerService.RegisterAdminAsync(signUpRequestDto);
            if (response == null)
                return NotFound(response);
            return Ok(response);
        }
    }
}
