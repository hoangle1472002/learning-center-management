namespace LearningCenter.Application.DTOs.Exam;

public class ExamListResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public DateTime ExamDate { get; set; }
    public int DurationMinutes { get; set; }
    public decimal TotalMarks { get; set; }
    public decimal? PassingMarks { get; set; }
    public string ExamType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Instructions { get; set; }
    public string? Location { get; set; }
    public DateTime CreatedAt { get; set; }
}