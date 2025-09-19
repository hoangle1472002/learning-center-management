using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Domain.Entities;

public class Class : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [MaxLength(20)]
    public string? Code { get; set; }
    
    [Required]
    public int SubjectId { get; set; }
    
    [Required]
    public int TeacherId { get; set; }
    
    public int MaxStudents { get; set; } = 30;
    public int MaxCapacity { get; set; } = 30; // Alias for MaxStudents
    
    public int CurrentStudents { get; set; } = 0;
    public int CurrentEnrollment { get; set; } = 0; // Alias for CurrentStudents
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public decimal? Price { get; set; }
    
    [MaxLength(50)]
    public string? Status { get; set; } = "Draft"; // Draft, Active, Completed, Cancelled
    
    [MaxLength(200)]
    public string? Room { get; set; }
    
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    // Navigation properties
    public virtual Subject Subject { get; set; } = null!;
    public virtual Teacher Teacher { get; set; } = null!;
    public virtual ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
    public virtual ICollection<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>();
    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
}
