namespace LearningCenter.Application.DTOs.Subject;

public class SubjectListResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Code { get; set; }
    public decimal Price { get; set; }
    public int Duration { get; set; }
    public string? Level { get; set; }
    public string? Prerequisites { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int ClassCount { get; set; }
    public int StudentCount { get; set; }
}
