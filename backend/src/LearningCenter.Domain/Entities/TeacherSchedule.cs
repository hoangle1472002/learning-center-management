using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Domain.Entities;

public class TeacherSchedule : BaseEntity
{
    [Required]
    public int TeacherId { get; set; }
    
    [Required]
    public DayOfWeek DayOfWeek { get; set; }
    
    [Required]
    public TimeOnly StartTime { get; set; }
    
    [Required]
    public TimeOnly EndTime { get; set; }
    
    [MaxLength(200)]
    public string? Notes { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual Teacher Teacher { get; set; } = null!;
}
