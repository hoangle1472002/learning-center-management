using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Application.DTOs.User;

public class UpdateUserRequest
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
    
    [StringLength(500)]
    public string? Address { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    
    [StringLength(10)]
    public string? Gender { get; set; }
    
    [StringLength(500)]
    public string? ProfileImageUrl { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public List<string> Roles { get; set; } = new();
}
