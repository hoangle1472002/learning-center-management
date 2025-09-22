using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Application.DTOs.Exam;

public class CreateExamRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public int ClassId { get; set; }

    [Required]
    public DateTime ExamDate { get; set; }

    [Required]
    [Range(1, 480, ErrorMessage = "Duration must be between 1 and 480 minutes")]
    public int DurationMinutes { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Total marks must be greater than 0")]
    public decimal TotalMarks { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Passing marks must be greater than or equal to 0")]
    public decimal? PassingMarks { get; set; }

    [MaxLength(50)]
    public string ExamType { get; set; } = "Regular";

    [MaxLength(500)]
    public string? Instructions { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }
}