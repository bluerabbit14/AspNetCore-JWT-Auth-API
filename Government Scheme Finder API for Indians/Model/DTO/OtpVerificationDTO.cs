namespace Asp_.Net_Web_Api.Model.DTO
{
    public class OtpVerificationDTO
    {
        public string Destination { get; set; } // email or phone
        public string Code { get; set; }
        public string Type { get; set; } // email or phone
    }
}
