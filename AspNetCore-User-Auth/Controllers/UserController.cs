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
    //I have modify it to display dynamic error msg. u can also change it to [authorize(Roles="User")] but it wont display dynamic error msg
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
            var roleFromToken = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role");

            if (string.IsNullOrEmpty(roleFromToken) || roleFromToken != "User")
            {
                return Forbid("Access denied: your role is not authorized to access this resource.");
            }

            if (!int.TryParse(userIdFromToken, out int userIdFromJwt))
            {
                return Unauthorized("Invalid or missing UserId claim in token.");
            }

            if (userIdFromJwt != id)
            {
                return Forbid("Access denied: you can only access your own profile.");
            }


            var user = await _userService.GetUserProfileAsync(id);
            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found." });
            return Ok(user);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateProfileByUserDTO dto, int id)
        {
            var userIdFromToken = User.FindFirstValue("UserId");
            var roleFromToken = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role");

            if (string.IsNullOrEmpty(roleFromToken) || roleFromToken != "User")
            {
                return Forbid("Access denied: your role is not authorized to access this resource.");
            }

            if (!int.TryParse(userIdFromToken, out int userIdFromJwt))
            {
                return Unauthorized("Invalid or missing UserId claim in token.");
            }

            if (userIdFromJwt != id)
            {
                return Forbid("Access denied: you can only access your own profile.");
            }

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
            var userIdFromToken = User.FindFirstValue("UserId");
            var roleFromToken = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role");

            if (string.IsNullOrEmpty(roleFromToken) || roleFromToken != "User")
            {
                return Forbid("Access denied: your role is not authorized to access this resource.");
            }

            if (!int.TryParse(userIdFromToken, out int userIdFromJwt))
            {
                return Unauthorized("Invalid or missing UserId claim in token.");
            }

            if (userIdFromJwt != id)
            {
                return Forbid("Access denied: you can only access your own profile.");
            }

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
