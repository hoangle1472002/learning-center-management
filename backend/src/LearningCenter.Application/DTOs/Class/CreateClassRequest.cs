using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Application.DTOs.Class;

public class CreateClassRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [StringLength(20)]
    public string? Code { get; set; }
    
    [Required]
    public int SubjectId { get; set; }
    
    [Required]
    public int TeacherId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int MaxStudents { get; set; } = 30;
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public decimal? Price { get; set; }
    
    [StringLength(50)]
    public string? Status { get; set; } = "Draft"; // Draft, Active, Completed, Cancelled
    
    [StringLength(200)]
    public string? Room { get; set; }
    
    [StringLength(500)]
    public string? Notes { get; set; }
}
