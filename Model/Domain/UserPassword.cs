using System.ComponentModel.DataAnnotations;

namespace Asp_.Net_Web_Api.Model.Domain
{
    public class UserPassword
    {
        [Key] public int PasswordId { get; set; } //primary key, auto-incremented
        public int UserId { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual UserProfile? userProfile { get; set; }
    }
}
