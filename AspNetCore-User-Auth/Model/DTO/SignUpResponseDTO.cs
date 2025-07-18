﻿using System.ComponentModel.DataAnnotations;

namespace Asp_.Net_Web_Api.Model.DTO
{
    public class SignUpResponseDTO
    {
        [Required] public string Message { get; set; }
        [Required] public int UserId { get; set; } 
        [Required,EmailAddress] public required string Email { get; set; } 
        [Required] public DateTime CreatedAt { get; set; }
    }
}
