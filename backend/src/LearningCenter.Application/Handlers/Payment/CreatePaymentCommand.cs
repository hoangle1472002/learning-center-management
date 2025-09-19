using LearningCenter.Application.DTOs.Payment;
using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using MediatR;

namespace LearningCenter.Application.Handlers.Payment;

public class CreatePaymentCommand : IRequest<PaymentListResponse>
{
    public CreatePaymentRequest Request { get; set; } = null!;
}

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, PaymentListResponse>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IClassRepository _classRepository;

    public CreatePaymentCommandHandler(
        IPaymentRepository paymentRepository,
        IStudentRepository studentRepository,
        IClassRepository classRepository)
    {
        _paymentRepository = paymentRepository;
        _studentRepository = studentRepository;
        _classRepository = classRepository;
    }

    public async Task<PaymentListResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        // Validate student exists
        var student = await _studentRepository.GetByIdAsync(request.Request.StudentId);
        if (student == null)
            throw new ArgumentException("Student not found");

        // Validate class exists
        var classEntity = await _classRepository.GetByIdAsync(request.Request.ClassId);
        if (classEntity == null)
            throw new ArgumentException("Class not found");

        var payment = new Domain.Entities.Payment
        {
            StudentId = request.Request.StudentId,
            ClassId = request.Request.ClassId,
            Amount = request.Request.Amount,
            DueDate = request.Request.DueDate,
            PaymentMethod = request.Request.PaymentMethod,
            Notes = request.Request.Notes,
            Status = "Pending"
        };

        await _paymentRepository.AddAsync(payment);
        await _paymentRepository.SaveChangesAsync();

        return new PaymentListResponse
        {
            Id = payment.Id,
            StudentId = payment.StudentId,
            StudentName = $"{student.FirstName} {student.LastName}",
            ClassId = payment.ClassId,
            ClassName = classEntity.Name,
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
