using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Application.DTOs.Teacher;

public class CreateTeacherRequest
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
    
    [Required]
    public DateTime DateOfBirth { get; set; }
    
    [StringLength(100)]
    public string? Specialization { get; set; }
    
    [StringLength(1000)]
    public string? Bio { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal HourlyRate { get; set; }
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}
