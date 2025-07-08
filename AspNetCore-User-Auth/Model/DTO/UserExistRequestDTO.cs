using System.ComponentModel.DataAnnotations;

namespace Asp_.Net_Web_Api.Model.DTO
{
    public class UserExistRequestDTO
    {
        [Required] public string Credential { get; set; } //either email or phone
    }
}
