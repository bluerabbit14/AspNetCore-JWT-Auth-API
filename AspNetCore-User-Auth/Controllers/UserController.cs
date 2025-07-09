using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.DTO;
using Asp_.Net_Web_Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Asp_.Net_Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("Fetch/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userIdFromToken = User.FindFirstValue("UserId");
            if (!int.TryParse(userIdFromToken, out int usedId) || usedId != id )
            {
                return Unauthorized(); // Or return Unauthorized()
            }
            var user = await _userService.GetUserProfileAsync(id);
            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found." });
            return Ok(user);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateProfileByUserDTO dto, int id)
        {
            var user = await _userService.UpdateUserProfileAsync(id, dto);
            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found." });

            return Ok(new
            {
                message = $"User with ID {id} Updated successfully.",
                user
            });
        }

        [HttpPatch("Edit/{id}")]
        public async Task<IActionResult> Patch([FromBody] UpdateProfileByUserDTO dto, int id)
        {
            var user = await _userService.PatchUserProfileAsync(id, dto);
            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found." });

            return Ok(new
            {
                message = $"User with ID {id} Updated successfully.",
                user
            });
        }
    }
}
