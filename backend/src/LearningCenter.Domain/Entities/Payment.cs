using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Domain.Entities;

public class Payment : BaseEntity
{
    [Required]
    public int StudentId { get; set; }
    
    [Required]
    public int ClassId { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
    
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = string.Empty; // Cash, Bank Transfer, Credit Card
    
    [MaxLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded
    
    [MaxLength(500)]
    public string? TransactionId { get; set; }
    
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    public DateTime? PaidAt { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    // Navigation properties
    public virtual Student Student { get; set; } = null!;
    public virtual Class Class { get; set; } = null!;
}
