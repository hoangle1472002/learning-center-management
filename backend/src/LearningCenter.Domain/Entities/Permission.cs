using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Domain.Entities;

public class Permission : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? Description { get; set; }
    
    [MaxLength(100)]
    public string? Resource { get; set; }
    
    [MaxLength(50)]
    public string? Action { get; set; }
    
    // Navigation properties
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
