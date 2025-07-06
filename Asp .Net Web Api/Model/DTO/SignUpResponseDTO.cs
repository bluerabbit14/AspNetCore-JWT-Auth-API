namespace Asp_.Net_Web_Api.Model.DTO
{
    public class SignUpResponseDTO
    {
        public required int UserId { get; set; } 
        public required string Email { get; set; } 
        public required DateTime CreatedAt { get; set; }
    }
}
