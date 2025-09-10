using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Domain.Entities;

public class UserRole : BaseEntity
{
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int RoleId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
}
