namespace Asp_.Net_Web_Api.Model.DTO
{
    public class SignUpRequestDTO
    {
        public required string Name { get; set; } 
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Password { get; set; }
        public required string Gender { get; set; } 
        public required string DateOfBirth { get; set; } //ISO 8601 format (yyyy-MM-dd)
    }
}