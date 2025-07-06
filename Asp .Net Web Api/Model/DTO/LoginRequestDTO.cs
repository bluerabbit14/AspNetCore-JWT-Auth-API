using Newtonsoft.Json;
using System.ComponentModel;
using System.Text.Json;

namespace Asp_.Net_Web_Api.Model.DTO
{
    public class LoginRequestDTO
    {
        public string Credentials { get; set; }
        public string Password { get; set; }
        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)] public bool RememberMe { get; set; }
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
    }
}
