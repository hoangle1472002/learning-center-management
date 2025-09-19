using System.ComponentModel.DataAnnotations;

namespace LearningCenter.Application.DTOs.Payment;

public class UpdatePaymentRequest
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [MaxLength(20)]
    public string? PaymentMethod { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
