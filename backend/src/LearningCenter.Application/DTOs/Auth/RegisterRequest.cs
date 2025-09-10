using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Application.DTOs.Auth;

public class RegisterRequest
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Address { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    
    [MaxLength(10)]
    public string? Gender { get; set; }
}
