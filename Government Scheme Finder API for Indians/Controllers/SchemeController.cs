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
    public class SchemeController : ControllerBase
    {
        private readonly ISchemeService _schemeService;
        private readonly IUserService _userService;

        public SchemeController(ISchemeService schemeService, IUserService userService)
        {
            _schemeService = schemeService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var schemes = await _schemeService.GetAllAsync();
            return Ok(schemes);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddScheme([FromBody] SchemeDTO dto)
        {
            var scheme = new Scheme
            {
                Title = dto.Title,
                Description = dto.Description,
                State = dto.State,
                Category = dto.Category,
                EligibilityCriteria = dto.EligibilityCriteria,
                Benefits = dto.Benefits
            };

            await _schemeService.AddSchemeAsync(scheme);
            return Ok("Scheme added successfully");
        }

        [Authorize(Roles = "User")]
        [HttpPost("recommend")]
        public async Task<IActionResult> Recommend()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userService.GetByIdAsync(userId);
            var schemes = await _schemeService.RecommendSchemesAsync(user);
            return Ok(schemes);
        }
    }
}
