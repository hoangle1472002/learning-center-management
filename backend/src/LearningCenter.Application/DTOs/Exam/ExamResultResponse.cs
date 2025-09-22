namespace LearningCenter.Application.DTOs.Exam;

public class ExamResultResponse
{
    public int Id { get; set; }
    public int ExamId { get; set; }
    public string ExamTitle { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public decimal? ObtainedMarks { get; set; }
    public decimal? Percentage { get; set; }
    public string? Grade { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? SubmittedAt { get; set; }
    public string? Remarks { get; set; }
    public bool IsPassed { get; set; }
    public DateTime CreatedAt { get; set; }
}
