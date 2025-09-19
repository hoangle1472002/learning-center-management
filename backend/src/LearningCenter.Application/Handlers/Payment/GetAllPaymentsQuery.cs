using LearningCenter.Application.DTOs.Payment;
using LearningCenter.Application.Interfaces;
using MediatR;

namespace LearningCenter.Application.Handlers.Payment;

public class GetAllPaymentsQuery : IRequest<IEnumerable<PaymentListResponse>>
{
    public string? Status { get; set; }
    public int? StudentId { get; set; }
    public int? ClassId { get; set; }
}

public class GetAllPaymentsQueryHandler : IRequestHandler<GetAllPaymentsQuery, IEnumerable<PaymentListResponse>>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetAllPaymentsQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<IEnumerable<PaymentListResponse>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
    {
        var payments = await _paymentRepository.GetAllAsync();
        
        var query = payments.AsQueryable();

        if (!string.IsNullOrEmpty(request.Status))
            query = query.Where(p => p.Status == request.Status);

        if (request.StudentId.HasValue)
            query = query.Where(p => p.StudentId == request.StudentId.Value);

        if (request.ClassId.HasValue)
            query = query.Where(p => p.ClassId == request.ClassId.Value);

        return query.Select(p => new PaymentListResponse
        {
            Id = p.Id,
            StudentId = p.StudentId,
            StudentName = $"{p.Student.FirstName} {p.Student.LastName}",
            ClassId = p.ClassId,
            ClassName = p.Class.Name,
            Amount = p.Amount,
            DueDate = p.DueDate,
            PaidDate = p.PaidDate,
            Status = p.Status,
            PaymentMethod = p.PaymentMethod,
            Notes = p.Notes,
            TransactionId = p.TransactionId,
            CreatedAt = p.CreatedAt
        }).OrderByDescending(p => p.CreatedAt);
    }
}
