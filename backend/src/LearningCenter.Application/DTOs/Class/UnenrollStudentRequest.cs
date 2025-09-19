using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Application.DTOs.Class;

public class UnenrollStudentRequest
{
    [Required]
    public int StudentId { get; set; }
    
    [Required]
    public int ClassId { get; set; }
}
