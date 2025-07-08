using Asp_.Net_Web_Api.Data;
using Asp_.Net_Web_Api.Infrastructure.Services;
using Asp_.Net_Web_Api.Model.Domain;
using Asp_.Net_Web_Api.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Asp_.Net_Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationController : ControllerBase
    {
        private readonly OtpService _otpService;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public VerificationController(OtpService otpService, UserManager<User> userManager, AppDbContext context)
        {
            _otpService = otpService;
            _userManager = userManager;
            _context = context;
        }

        [HttpPost("send-otp")]
        public IActionResult SendOtp([FromBody] OtpVerificationDTO dto)
        {
            _otpService.GenerateAndSendOtp(dto.Destination, dto.Type);
            return Ok("OTP sent");
        }

        [Authorize]
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationDTO dto)
        {
            if (!_otpService.VerifyOtp(dto.Destination, dto.Code))
                return BadRequest("Invalid OTP");

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();

            if (dto.Type == "email") user.IsEmailVerified = true;
            else if (dto.Type == "phone") user.IsPhoneVerified = true;

            await _userManager.UpdateAsync(user);
            return Ok("Verification successful");
        }
    }
}
