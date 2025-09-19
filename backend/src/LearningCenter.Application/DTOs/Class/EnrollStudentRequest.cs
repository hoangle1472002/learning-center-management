using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Application.DTOs.Class;

public class EnrollStudentRequest
{
    [Required]
    public int StudentId { get; set; }
    
    [Required]
    public int ClassId { get; set; }
    
    public DateTime? EnrollmentDate { get; set; }
    
    [StringLength(500)]
    public string? Notes { get; set; }
}
