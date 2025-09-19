using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningCenter.Domain.Entities;

public class Payment : BaseEntity
{
    [Required]
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    [Required]
    public int ClassId { get; set; }
    public Class Class { get; set; } = null!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    public DateTime? PaidDate { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Paid, Overdue, Cancelled

    [MaxLength(20)]
    public string? PaymentMethod { get; set; } // Cash, Bank Transfer, Credit Card, etc.

    [MaxLength(500)]
    public string? Notes { get; set; }

    [MaxLength(100)]
    public string? TransactionId { get; set; }

    [MaxLength(200)]
    public string? ReceiptUrl { get; set; }

    // Navigation properties
    public ICollection<PaymentHistory> PaymentHistories { get; set; } = new List<PaymentHistory>();
}