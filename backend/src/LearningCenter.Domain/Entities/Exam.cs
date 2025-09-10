using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Domain.Entities;

public class Exam : BaseEntity
{
    [Required]
    public int ClassId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public DateTime ExamDate { get; set; }
    
    public int Duration { get; set; } // in minutes
    
    public int TotalMarks { get; set; }
    
    public int PassingMarks { get; set; }
    
    [MaxLength(50)]
    public string Type { get; set; } = "Regular"; // Regular, Midterm, Final
    
    [MaxLength(50)]
    public string Status { get; set; } = "Draft"; // Draft, Published, Completed, Cancelled
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual Class Class { get; set; } = null!;
    public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
}
