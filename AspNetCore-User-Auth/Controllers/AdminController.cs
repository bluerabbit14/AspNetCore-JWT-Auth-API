using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Asp_.Net_Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    //I have modify it to display dynamic error msg. u can also change it to [authorize(Roles="admin")] but it wont display dynamic error msg
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
            var roleFromToken = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role");
            if (string.IsNullOrEmpty(roleFromToken) || roleFromToken != "admin")
            {
                return Forbid("Access denied: your role is not authorized to access this resource.");
            }

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

        // PATCH: api/Admin/AlterActiveStatus/{id}?status=true
        [HttpPatch("AlterActiveStatus/{id}")]
        public async Task<IActionResult> AlterActiveStatus(int id, [FromQuery] bool status)
        {
            try
            {
                var result = await _adminService.AlterUserActiveStatus(id, status);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE: api/Admin/DeleteUserProfile/{id}
        [HttpDelete("DeleteUserProfile/{id}")]
        public async Task<IActionResult> DeleteUserProfile(int id)
        {
            try
            {
                var result = await _adminService.DeleteUserProfileAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET: api/Admin/UserProfileByEmail?email=example@email.com
        [HttpGet("UserProfileByEmail")]
        public async Task<IActionResult> GetUserProfileByEmail([FromQuery] string email)
        {
            try
            {
                var user = await _adminService.GetUserProfileByEmailAsync(email);
                return Ok(user);
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

        // GET: api/Admin/UserProfileByPhone?phone=1234567890
        [HttpGet("UserProfileByPhone")]
        public async Task<IActionResult> GetUserProfileByPhone([FromQuery] string phone)
        {
            try
            {
                var user = await _adminService.GetUserProfileByPhoneAsync(phone);
                return Ok(user);
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

        // GET: api/Admin/UserProfilesByGender?gender=male
        [HttpGet("UserProfilesByGender")]
        public async Task<IActionResult> GetUserProfilesByGender([FromQuery] string gender)
        {
            try
            {
                var users = await _adminService.GetUserProfileByGenderAsync(gender);
                return Ok(users);
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
