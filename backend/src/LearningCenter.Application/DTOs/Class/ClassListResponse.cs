namespace LearningCenter.Application.DTOs.Class;

public class ClassListResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Code { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public int MaxStudents { get; set; }
    public int CurrentStudents { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Price { get; set; }
    public string? Status { get; set; }
    public string? Room { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int EnrollmentCount { get; set; }
    public double EnrollmentRate => MaxStudents > 0 ? (double)CurrentStudents / MaxStudents * 100 : 0;
}
