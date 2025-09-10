using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Domain.Entities;

public class RefreshToken : BaseEntity
{
    [Required]
    public int UserId { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string Token { get; set; } = string.Empty;
    
    [Required]
    public DateTime ExpiresAt { get; set; }
    
    public bool IsRevoked { get; set; } = false;
    
    public DateTime? RevokedAt { get; set; }
    
    [MaxLength(200)]
    public string? RevokedByIp { get; set; }
    
    [MaxLength(500)]
    public string? ReplacedByToken { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    
    public bool IsActive => !IsRevoked && !IsExpired;
}
