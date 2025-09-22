using LearningCenter.Application.DTOs.Exam;
using LearningCenter.Application.Interfaces;
using MediatR;

namespace LearningCenter.Application.Handlers.Exam;

public class GetAllExamsQuery : IRequest<IEnumerable<ExamListResponse>>
{
    public string? Status { get; set; }
    public int? ClassId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetAllExamsQueryHandler : IRequestHandler<GetAllExamsQuery, IEnumerable<ExamListResponse>>
{
    private readonly IExamRepository _examRepository;

    public GetAllExamsQueryHandler(IExamRepository examRepository)
    {
        _examRepository = examRepository;
    }

    public async Task<IEnumerable<ExamListResponse>> Handle(GetAllExamsQuery request, CancellationToken cancellationToken)
    {
        var exams = await _examRepository.GetAllAsync();
        
        var query = exams.AsQueryable();

        if (!string.IsNullOrEmpty(request.Status))
            query = query.Where(e => e.Status == request.Status);

        if (request.ClassId.HasValue)
            query = query.Where(e => e.ClassId == request.ClassId.Value);

        if (request.StartDate.HasValue)
            query = query.Where(e => e.ExamDate >= request.StartDate.Value);

        if (request.EndDate.HasValue)
            query = query.Where(e => e.ExamDate <= request.EndDate.Value);

        return query.Select(e => new ExamListResponse
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            ClassId = e.ClassId,
            ClassName = e.Class.Name,
            ExamDate = e.ExamDate,
            DurationMinutes = e.DurationMinutes,
            TotalMarks = e.TotalMarks,
            PassingMarks = e.PassingMarks,
            ExamType = e.ExamType,
            Status = e.Status,
            Instructions = e.Instructions,
            Location = e.Location,
            CreatedAt = e.CreatedAt
        }).OrderBy(e => e.ExamDate);
    }
}
