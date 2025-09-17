using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Application.DTOs.Student;

public class CreateStudentRequest
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
    public string? ParentName { get; set; }
    
    [StringLength(20)]
    public string? ParentPhone { get; set; }
    
    [StringLength(500)]
    public string? Notes { get; set; }
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}
