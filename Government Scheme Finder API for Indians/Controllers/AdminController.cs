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
        [HttpPatch("UpdateProfile/{id}")]
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
    }
}
