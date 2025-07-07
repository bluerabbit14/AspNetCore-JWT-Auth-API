using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp_.Net_Web_Api.Model.DTO
{
    public class GetUserDTO
    {
        public string? ImageUrl { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public string? Pincode { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string? Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }
        public string? LanguagePreference { get; set; }
        public string? Timezone { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public string? Bio { get; set; }
        public string? SocialHandle { get; set; }
    }
}
