using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Application.DTOs.Auth;

public class SocialLoginRequest
{
    [Required]
    public string Provider { get; set; } = string.Empty; // "Google" or "Facebook"
    
    [Required]
    public string IdToken { get; set; } = string.Empty;
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? ProfilePicture { get; set; }
}
