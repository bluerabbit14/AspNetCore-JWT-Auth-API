using System.ComponentModel.DataAnnotations;

namespace Asp_.Net_Web_Api.Model.DTO
{
    public class SignUpRequestDTO
    {
        [Required] public required string Name { get; set; }
        [Required, EmailAddress] public required string Email { get; set; }
        [Required, Phone] public required string Phone { get; set; }
        [Required] public required string Password { get; set; }
        [Required] public required string Gender { get; set; }
        [Required] public required string DateOfBirth { get; set; } //ISO 8601 format (yyyy-MM-dd)
    }
}