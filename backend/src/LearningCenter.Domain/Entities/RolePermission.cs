using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Domain.Entities;

public class RolePermission : BaseEntity
{
    [Required]
    public int RoleId { get; set; }
    
    [Required]
    public int PermissionId { get; set; }
    
    // Navigation properties
    public virtual Role Role { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;
}
