using System.ComponentModel.DataAnnotations;

namespace Asp_.Net_Web_Api.Model.DTO
{
    public class UserExistResponseDTO
    {
        [Required]public string Name { get; set; }
        [Required]public string Role { get; set; }
        [Required] public bool IsCredentialVerified { get; set; }
        [Required] public DateTime CreatedAt { get; set; }
        [Required] public bool IsActive { get; set; }
    }
}