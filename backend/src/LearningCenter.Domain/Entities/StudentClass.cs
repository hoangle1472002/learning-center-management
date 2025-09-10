using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Domain.Entities;

public class StudentClass : BaseEntity
{
    [Required]
    public int StudentId { get; set; }
    
    [Required]
    public int ClassId { get; set; }
    
    public DateTime? EnrollmentDate { get; set; }
    
    public DateTime? CompletionDate { get; set; }
    
    [MaxLength(50)]
    public string Status { get; set; } = "Enrolled"; // Enrolled, Completed, Dropped, Suspended
    
    public decimal? PaidAmount { get; set; }
    
    public decimal? RemainingAmount { get; set; }
    
    // Navigation properties
    public virtual Student Student { get; set; } = null!;
    public virtual Class Class { get; set; } = null!;
}
