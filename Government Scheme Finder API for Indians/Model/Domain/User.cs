using Microsoft.AspNetCore.Identity;

namespace Asp_.Net_Web_Api.Model.Domain
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal AnnualIncome { get; set; }
        public string Profession { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Bio { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
    }
}
