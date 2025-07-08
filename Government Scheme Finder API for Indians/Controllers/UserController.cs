using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.Domain;
using Asp_.Net_Web_Api.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Asp_.Net_Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userService.GetByIdAsync(userId);
            if (user == null) return NotFound();

            var dto = new UserProfileDTO
            {
                FullName = user.FullName,
                Age = user.Age,
                Gender = user.Gender,
                AnnualIncome = user.AnnualIncome,
                Profession = user.Profession,
                State = user.State
            };

            return Ok(dto);
        }
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDTO dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = new User
            {
                Id = userId,
                FullName = dto.FullName,
                Age = dto.Age,
                Gender = dto.Gender,
                AnnualIncome = dto.AnnualIncome,
                Profession = dto.Profession,
                State = dto.State
            };

            await _userService.UpdateProfileAsync(user);
            return Ok("Profile updated successfully");
        }
    }
}
