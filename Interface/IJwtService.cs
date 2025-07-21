using Asp_.Net_Web_Api.Model.Domain;

namespace Asp_.Net_Web_Api.Interface
{
    public interface IJwtService
    {
        public string GenerateToken(UserProfile user);
        public DateTime? GetTokenExpiration(string token);
    }
}
