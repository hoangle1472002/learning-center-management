using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningCenter.Domain.Entities;

public class ExamAnswer : BaseEntity
{
    [Required]
    public int ExamResultId { get; set; }
    public ExamResult ExamResult { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Question { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Answer { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? Marks { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? MaxMarks { get; set; }

    [MaxLength(500)]
    public string? Feedback { get; set; }

    public int QuestionNumber { get; set; }

    [MaxLength(50)]
    public string QuestionType { get; set; } = "Text"; // Text, MultipleChoice, TrueFalse, Essay
}
