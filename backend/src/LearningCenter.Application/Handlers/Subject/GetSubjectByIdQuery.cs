using LearningCenter.Application.DTOs.Subject;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Subject;

public class GetSubjectByIdQuery : IRequest<SubjectListResponse?>
{
    public int Id { get; set; }
}

public class GetSubjectByIdQueryHandler : IRequestHandler<GetSubjectByIdQuery, SubjectListResponse?>
{
    private readonly ISubjectRepository _subjectRepository;
    private readonly ILogger<GetSubjectByIdQueryHandler> _logger;

    public GetSubjectByIdQueryHandler(
        ISubjectRepository subjectRepository,
        ILogger<GetSubjectByIdQueryHandler> logger)
    {
        _subjectRepository = subjectRepository;
        _logger = logger;
    }

    public async Task<SubjectListResponse?> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting subject by id {SubjectId}", request.Id);

            var subject = await _subjectRepository.GetByIdAsync(request.Id);
            
            if (subject == null)
            {
                _logger.LogWarning("Subject with id {SubjectId} not found", request.Id);
                return null;
            }

            var result = new SubjectListResponse
            {
                Id = subject.Id,
                Name = subject.Name,
                Description = subject.Description,
                Code = subject.Code,
                Price = subject.Price,
                Duration = subject.Duration,
                Level = subject.Level,
                Prerequisites = subject.Prerequisites,
                IsActive = subject.IsActive,
                CreatedAt = subject.CreatedAt,
                UpdatedAt = subject.UpdatedAt,
                ClassCount = subject.Classes?.Count ?? 0,
                StudentCount = subject.Classes?.SelectMany(c => c.StudentClasses).Count() ?? 0
            };

            _logger.LogInformation("Subject {SubjectId} retrieved successfully", request.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting subject by id {SubjectId}", request.Id);
            throw;
        }
    }
}
