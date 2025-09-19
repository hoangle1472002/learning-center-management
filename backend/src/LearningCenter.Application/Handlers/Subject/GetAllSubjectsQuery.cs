using LearningCenter.Application.DTOs.Subject;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Subject;

public class GetAllSubjectsQuery : IRequest<IEnumerable<SubjectListResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? Level { get; set; }
    public bool? IsActive { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}

public class GetAllSubjectsQueryHandler : IRequestHandler<GetAllSubjectsQuery, IEnumerable<SubjectListResponse>>
{
    private readonly ISubjectRepository _subjectRepository;
    private readonly ILogger<GetAllSubjectsQueryHandler> _logger;

    public GetAllSubjectsQueryHandler(
        ISubjectRepository subjectRepository,
        ILogger<GetAllSubjectsQueryHandler> logger)
    {
        _subjectRepository = subjectRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<SubjectListResponse>> Handle(GetAllSubjectsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting all subjects with page {PageNumber}, size {PageSize}", 
                request.PageNumber, request.PageSize);

            var subjects = await _subjectRepository.GetAllAsync();
            
            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                subjects = subjects.Where(s => 
                    s.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (s.Description != null && s.Description.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (s.Code != null && s.Code.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrEmpty(request.Level))
            {
                subjects = subjects.Where(s => s.Level != null && 
                    s.Level.Contains(request.Level, StringComparison.OrdinalIgnoreCase));
            }

            if (request.IsActive.HasValue)
            {
                subjects = subjects.Where(s => s.IsActive == request.IsActive.Value);
            }

            if (request.MinPrice.HasValue)
            {
                subjects = subjects.Where(s => s.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                subjects = subjects.Where(s => s.Price <= request.MaxPrice.Value);
            }

            // Apply pagination
            var pagedSubjects = subjects
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            var result = pagedSubjects.Select(s => new SubjectListResponse
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Code = s.Code,
                Price = s.Price,
                Duration = s.Duration,
                Level = s.Level,
                Prerequisites = s.Prerequisites,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                ClassCount = s.Classes?.Count ?? 0,
                StudentCount = s.Classes?.SelectMany(c => c.StudentClasses).Count() ?? 0
            });

            _logger.LogInformation("Retrieved {Count} subjects", result.Count());
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all subjects");
            throw;
        }
    }
}
