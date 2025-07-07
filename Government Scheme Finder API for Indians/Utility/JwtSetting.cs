namespace Asp_.Net_Web_Api.Utility
{
    public class JwtSetting
    {
        public static string SecretKey { get; set; } = "YourSuperSecretKeyForJWTTokenGeneration";
        public static string Issuer { get; set; } = "MySchemeFinder";
        public static string Audience { get; set; } = "MyAudience";
        public static int ExpirationMinutes { get; set; } = 1440; // 24 hours
        public static bool ValidateIssuerSigningKey { get; set; } = true;
        public static bool ValidateIssuer { get; set; } = true;
        public static bool ValidateAudience { get; set; } = true;
        public static bool RequireExpirationTime { get; set; } = true;
    }
}
