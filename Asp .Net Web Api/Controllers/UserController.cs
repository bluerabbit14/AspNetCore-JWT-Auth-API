using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Asp_.Net_Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("FetchUser/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.GetUserProfileAsync(id);
            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found." });
            return Ok(user);
        }
        [HttpPut("UpdateProfile/{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateProfileDTO dto, int id)
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
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound(new { message = $"User with ID {id} not found." });

            return Ok(new { message = $"User with ID {id} deleted successfully." });
        }
    }
}
