using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.Domain;
using Microsoft.IdentityModel.Tokens;

namespace Asp_.Net_Web_Api.Utility
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        public JwtService(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateToken(UserProfile user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var tokenValidityMins = _config.GetValue<int>("Jwt:TokenValidityMins");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Name ?? string.Empty),
                    new Claim("UserId", user.UserId.ToString()), // Custom claim for userId
                    new Claim(ClaimTypes.Role, user.Role.Trim().ToLower() == "user" ? "User" : user.Role),
                    new Claim(ClaimTypes.Gender, user.Gender),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.PhoneNumber, user.Phone ?? string.Empty)
                }),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = creds, 
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
        public DateTime? GetTokenExpiration(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
                return null;
            var jwtToken = handler.ReadJwtToken(token);
            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
            if (expClaim != null && long.TryParse(expClaim.Value, out long expUnix))
            {
                // Convert Unix time to UTC DateTime  
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                return expirationTime;
            }
            return null;
        }
    }
}
