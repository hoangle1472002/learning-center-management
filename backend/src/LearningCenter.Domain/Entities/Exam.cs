using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningCenter.Domain.Entities;

public class Exam : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public int ClassId { get; set; }
    public Class Class { get; set; } = null!;

    [Required]
    public DateTime ExamDate { get; set; }

    [Required]
    public int DurationMinutes { get; set; } // Exam duration in minutes

    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public decimal TotalMarks { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? PassingMarks { get; set; }

    [MaxLength(50)]
    public string ExamType { get; set; } = "Regular"; // Regular, Midterm, Final, Quiz

    [MaxLength(50)]
    public string Status { get; set; } = "Scheduled"; // Scheduled, InProgress, Completed, Cancelled

    [MaxLength(500)]
    public string? Instructions { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    // Navigation properties
    public ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
}