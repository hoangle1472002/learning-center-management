using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningCenter.Domain.Entities;

public class ExamResult : BaseEntity
{
    [Required]
    public int ExamId { get; set; }
    public Exam Exam { get; set; } = null!;

    [Required]
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    [Column(TypeName = "decimal(5,2)")]
    public decimal? ObtainedMarks { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? Percentage { get; set; }

    [MaxLength(10)]
    public string? Grade { get; set; } // A+, A, B+, B, C+, C, D, F

    [MaxLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Completed, Absent, Disqualified

    public DateTime? SubmittedAt { get; set; }

    [MaxLength(500)]
    public string? Remarks { get; set; }

    public bool IsPassed { get; set; } = false;

    // Navigation properties
    public ICollection<ExamAnswer> ExamAnswers { get; set; } = new List<ExamAnswer>();
}