using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Domain.Entities;

public class ClassSchedule : BaseEntity
{
    [Required]
    public int ClassId { get; set; }
    
    [Required]
    public DayOfWeek DayOfWeek { get; set; }
    
    [Required]
    public TimeOnly StartTime { get; set; }
    
    [Required]
    public TimeOnly EndTime { get; set; }
    
    [MaxLength(200)]
    public string? Room { get; set; }
    
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual Class Class { get; set; } = null!;
}
