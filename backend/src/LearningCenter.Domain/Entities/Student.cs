using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Domain.Entities;

public class Student : BaseEntity
{
    [Required]
    public int UserId { get; set; }
    
    [MaxLength(20)]
    public string? StudentCode { get; set; }
    
    [MaxLength(200)]
    public string? ParentName { get; set; }
    
    [MaxLength(20)]
    public string? ParentPhone { get; set; }
    
    [MaxLength(500)]
    public string? ParentEmail { get; set; }
    
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    public DateTime? EnrollmentDate { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
}
