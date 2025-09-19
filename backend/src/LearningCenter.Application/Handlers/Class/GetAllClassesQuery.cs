using LearningCenter.Application.DTOs.Class;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Class;

public class GetAllClassesQuery : IRequest<IEnumerable<ClassListResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public int? SubjectId { get; set; }
    public int? TeacherId { get; set; }
    public string? Status { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
}

public class GetAllClassesQueryHandler : IRequestHandler<GetAllClassesQuery, IEnumerable<ClassListResponse>>
{
    private readonly IClassRepository _classRepository;
    private readonly ILogger<GetAllClassesQueryHandler> _logger;

    public GetAllClassesQueryHandler(
        IClassRepository classRepository,
        ILogger<GetAllClassesQueryHandler> logger)
    {
        _classRepository = classRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ClassListResponse>> Handle(GetAllClassesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting all classes with page {PageNumber}, size {PageSize}", 
                request.PageNumber, request.PageSize);

            var classes = await _classRepository.GetAllAsync();
            
            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                classes = classes.Where(c => 
                    c.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (c.Description != null && c.Description.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (c.Code != null && c.Code.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    c.Subject.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (c.Teacher.FirstName + " " + c.Teacher.LastName).Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (request.SubjectId.HasValue)
            {
                classes = classes.Where(c => c.SubjectId == request.SubjectId.Value);
            }

            if (request.TeacherId.HasValue)
            {
                classes = classes.Where(c => c.TeacherId == request.TeacherId.Value);
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                classes = classes.Where(c => c.Status != null && 
                    c.Status.Contains(request.Status, StringComparison.OrdinalIgnoreCase));
            }

            if (request.IsActive.HasValue)
            {
                classes = classes.Where(c => c.IsActive == request.IsActive.Value);
            }

            if (request.StartDateFrom.HasValue)
            {
                classes = classes.Where(c => c.StartDate >= request.StartDateFrom.Value);
            }

            if (request.StartDateTo.HasValue)
            {
                classes = classes.Where(c => c.StartDate <= request.StartDateTo.Value);
            }

            // Apply pagination
            var pagedClasses = classes
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            var result = pagedClasses.Select(c => new ClassListResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Code = c.Code,
                SubjectName = c.Subject.Name,
                TeacherName = $"{c.Teacher.FirstName} {c.Teacher.LastName}",
                MaxStudents = c.MaxStudents,
                CurrentStudents = c.CurrentStudents,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Price = c.Price,
                Status = c.Status,
                Room = c.Room,
                Notes = c.Notes,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                EnrollmentCount = c.StudentClasses?.Count ?? 0
            });

            _logger.LogInformation("Retrieved {Count} classes", result.Count());
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all classes");
            throw;
        }
    }
}
