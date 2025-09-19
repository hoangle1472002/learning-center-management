using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Application.DTOs.Subject;

public class CreateSubjectRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [StringLength(20)]
    public string? Code { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Duration { get; set; } // Duration in hours
    
    [StringLength(50)]
    public string? Level { get; set; } // Beginner, Intermediate, Advanced
    
    [StringLength(200)]
    public string? Prerequisites { get; set; }
    
    public bool IsActive { get; set; } = true;
}
