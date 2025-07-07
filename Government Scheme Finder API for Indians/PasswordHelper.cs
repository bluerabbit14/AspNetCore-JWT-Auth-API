using System.Security.Cryptography;
using System.Text;

namespace Asp_.Net_Web_Api
{
    public class PasswordHelper
    {
        public static string GenerateSalt()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        }
        public static string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var combined = password + salt;
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
            return Convert.ToBase64String(hash);
        }
        public static bool VerifyPassword(string password, string storedHash, string salt)
        {
            var hashOfInput = HashPassword(password, salt);
            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(storedHash),
                Encoding.UTF8.GetBytes(hashOfInput)
            );
        }
    }
}
//For production apps, use PBKDF2, BCrypt, or Argon2 instead of SHA256.