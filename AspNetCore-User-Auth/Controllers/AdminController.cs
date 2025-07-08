using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Asp_.Net_Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET: api/Admin/AllUserProfiles
        [HttpGet("AllUserProfiles")]
        public async Task<IActionResult> GetAllUserProfiles()
        {
            var profiles = await _adminService.GetAllUserProfilesAsync();
            return Ok(profiles);
        }

        // GET: api/Admin/UserProfile/{id}
        [HttpGet("UserProfile/{id}")]
        public async Task<IActionResult> GetUserProfile(int id)
        {
            try
            {
                var user = await _adminService.GetUserProfileAsync(id);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPatch("Edit/{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateProfileByAdminDTO dto, int id)
        {
            var user = await _adminService.UpdateUserProfileByAdminAsync(id, dto);
            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found." });

            return Ok(new
            {
                message = $"User with ID {id} Updated successfully.",
                user
            });
        }

        // GET: api/Admin/UserProfilesByRole?role=admin&userId=1
        [HttpGet("UserProfilesByRole")]
        public async Task<IActionResult> GetUserProfilesByRole([FromQuery] string role, [FromQuery] int? userId)
        {
            try
            {
                var profiles = await _adminService.GetUserProfileByRoleAsync(role, userId);
                return Ok(profiles);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
