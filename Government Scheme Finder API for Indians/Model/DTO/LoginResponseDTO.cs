using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp_.Net_Web_Api.Model.DTO
{
    public class LoginResponseDTO
    {
         [Required]public string Message { get; set; }
         [Required]public int UserId { get; set; }
         [Required] public string Token { get; set; }
         [Required] public DateTime TokenValidity { get; set; }
    }
}
