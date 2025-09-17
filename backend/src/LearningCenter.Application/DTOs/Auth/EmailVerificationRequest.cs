using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Application.DTOs.Auth;

public class EmailVerificationRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Token { get; set; } = string.Empty;
}
