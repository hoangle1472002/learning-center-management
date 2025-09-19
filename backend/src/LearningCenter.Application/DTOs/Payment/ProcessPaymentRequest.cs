using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Application.DTOs.Payment;

public class ProcessPaymentRequest
{
    [Required]
    [MaxLength(20)]
    public string PaymentMethod { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? TransactionId { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    [MaxLength(200)]
    public string? ReceiptUrl { get; set; }
}
