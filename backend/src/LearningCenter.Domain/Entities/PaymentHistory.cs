using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningCenter.Domain.Entities;

public class PaymentHistory : BaseEntity
{
    [Required]
    public int PaymentId { get; set; }
    public Payment Payment { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? PaymentMethod { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Amount { get; set; }

    [MaxLength(100)]
    public string? TransactionId { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public DateTime? ProcessedAt { get; set; }

    [MaxLength(100)]
    public string? ProcessedBy { get; set; } // User who processed the payment
}
