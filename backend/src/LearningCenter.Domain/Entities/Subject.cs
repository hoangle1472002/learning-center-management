using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Domain.Entities;

public class Subject : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [MaxLength(20)]
    public string? Code { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Duration { get; set; } // in hours
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
    
    [MaxLength(50)]
    public string? Level { get; set; } // Beginner, Intermediate, Advanced
    
    [MaxLength(200)]
    public string? Prerequisites { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
