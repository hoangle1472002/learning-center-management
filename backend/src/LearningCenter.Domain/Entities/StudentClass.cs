using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Domain.Entities;

public class StudentClass : BaseEntity
{
    [Required]
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
    
    [Required]
    public int ClassId { get; set; }
    public Class Class { get; set; } = null!;
    
    [Required]
    public DateTime EnrollmentDate { get; set; }
    
    public DateTime? CompletionDate { get; set; }
    
    public bool IsActive { get; set; } = true;
}