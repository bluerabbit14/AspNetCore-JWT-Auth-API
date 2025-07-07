using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Asp_.Net_Web_Api.Model.DTO
{
    public class LoginRequestDTO
    {
        [Required] public string Credential { get; set; }
        [Required] public string Password { get; set; }
        [DefaultValue(false)][JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)] public bool RememberMe { get; set; }
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
    }
}
