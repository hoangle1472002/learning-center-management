using LearningCenter.Application.DTOs.Payment;
using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using MediatR;

namespace LearningCenter.Application.Handlers.Payment;

public class ProcessPaymentCommand : IRequest<PaymentListResponse>
{
    public int PaymentId { get; set; }
    public ProcessPaymentRequest Request { get; set; } = null!;
}

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, PaymentListResponse>
{
    private readonly IPaymentRepository _paymentRepository;

    public ProcessPaymentCommandHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<PaymentListResponse> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId);
        if (payment == null)
            throw new ArgumentException("Payment not found");

        if (payment.Status == "Paid")
            throw new InvalidOperationException("Payment has already been processed");

        // Create payment history
        var paymentHistory = new PaymentHistory
        {
            PaymentId = payment.Id,
            Status = "Paid",
            PaymentMethod = request.Request.PaymentMethod,
            Amount = payment.Amount,
            TransactionId = request.Request.TransactionId,
            Notes = request.Request.Notes,
            ProcessedAt = DateTime.UtcNow,
            ProcessedBy = "System" // In real app, get from current user context
        };

        // Update payment
        payment.Status = "Paid";
        payment.PaidDate = DateTime.UtcNow;
        payment.PaymentMethod = request.Request.PaymentMethod;
        payment.TransactionId = request.Request.TransactionId;
        payment.Notes = request.Request.Notes;
        payment.ReceiptUrl = request.Request.ReceiptUrl;

        await _paymentRepository.UpdateAsync(payment);
        await _paymentRepository.SaveChangesAsync();

        return new PaymentListResponse
        {
            Id = payment.Id,
            StudentId = payment.StudentId,
            StudentName = $"{payment.Student.FirstName} {payment.Student.LastName}",
            ClassId = payment.ClassId,
            ClassName = payment.Class.Name,
            Amount = payment.Amount,
            DueDate = payment.DueDate,
            PaidDate = payment.PaidDate,
            Status = payment.Status,
            PaymentMethod = payment.PaymentMethod,
            Notes = payment.Notes,
            TransactionId = payment.TransactionId,
            CreatedAt = payment.CreatedAt
        };
    }
}
