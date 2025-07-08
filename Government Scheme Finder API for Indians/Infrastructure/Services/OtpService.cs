using System.Collections.Concurrent;

namespace Asp_.Net_Web_Api.Infrastructure.Services
{
    public class OtpService
    {
        private readonly ConcurrentDictionary<string, string> _otpStorage = new();

        public string GenerateAndSendOtp(string destination, string type)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            _otpStorage[destination] = otp;

            // Replace with actual email/SMS integration
            Console.WriteLine($"[OTP {type}] Sending to {destination}: {otp}");

            return otp;
        }

        public bool VerifyOtp(string destination, string code)
        {
            return _otpStorage.TryGetValue(destination, out var correctCode) && correctCode == code;
        }
    }
}
