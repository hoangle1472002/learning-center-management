using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Domain.Entities;

public class Schedule : BaseEntity
{
    [Required]
    public int ClassId { get; set; }
    public Class Class { get; set; } = null!;
    
    [Required]
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;
    
    [Required]
    [StringLength(50)]
    public string DayOfWeek { get; set; } = string.Empty;
    
    [Required]
    public TimeSpan StartTime { get; set; }
    
    [Required]
    public TimeSpan EndTime { get; set; }
    
    [StringLength(200)]
    public string? Room { get; set; }
    
    public bool IsActive { get; set; } = true;
}
