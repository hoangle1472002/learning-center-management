using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Domain.Entities;

public class Teacher : BaseEntity
{
    [Required]
    public int UserId { get; set; }
    
    [MaxLength(20)]
    public string? EmployeeCode { get; set; }
    
    [MaxLength(200)]
    public string? Specialization { get; set; }
    
    [MaxLength(1000)]
    public string? Bio { get; set; }
    
    [MaxLength(200)]
    public string? Education { get; set; }
    
    [MaxLength(200)]
    public string? Experience { get; set; }
    
    public decimal? HourlyRate { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime? HireDate { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
    public virtual ICollection<TeacherSchedule> TeacherSchedules { get; set; } = new List<TeacherSchedule>();
}
