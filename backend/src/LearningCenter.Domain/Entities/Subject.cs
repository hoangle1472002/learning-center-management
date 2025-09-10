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
    
    public int? Duration { get; set; } // in hours
    
    public decimal? Price { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
