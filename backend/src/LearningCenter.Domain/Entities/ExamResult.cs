using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Domain.Entities;

public class ExamResult : BaseEntity
{
    [Required]
    public int ExamId { get; set; }
    
    [Required]
    public int StudentId { get; set; }
    
    public int? ObtainedMarks { get; set; }
    
    [MaxLength(10)]
    public string? Grade { get; set; }
    
    [MaxLength(500)]
    public string? Remarks { get; set; }
    
    public DateTime? SubmittedAt { get; set; }
    
    public DateTime? GradedAt { get; set; }
    
    [MaxLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Submitted, Graded
    
    // Navigation properties
    public virtual Exam Exam { get; set; } = null!;
    public virtual Student Student { get; set; } = null!;
    
    public bool IsPassed => ObtainedMarks.HasValue && ObtainedMarks >= Exam.PassingMarks;
}