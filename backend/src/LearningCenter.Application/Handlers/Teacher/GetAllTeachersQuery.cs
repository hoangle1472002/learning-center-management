using LearningCenter.Application.DTOs.Teacher;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Teacher;

public class GetAllTeachersQuery : IRequest<IEnumerable<TeacherListResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? Specialization { get; set; }
    public bool? IsActive { get; set; }
    public decimal? MinHourlyRate { get; set; }
    public decimal? MaxHourlyRate { get; set; }
}

public class GetAllTeachersQueryHandler : IRequestHandler<GetAllTeachersQuery, IEnumerable<TeacherListResponse>>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly ILogger<GetAllTeachersQueryHandler> _logger;

    public GetAllTeachersQueryHandler(
        ITeacherRepository teacherRepository,
        ILogger<GetAllTeachersQueryHandler> logger)
    {
        _teacherRepository = teacherRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<TeacherListResponse>> Handle(GetAllTeachersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting all teachers with page {PageNumber}, size {PageSize}", 
                request.PageNumber, request.PageSize);

            var teachers = await _teacherRepository.GetAllAsync();
            
            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                teachers = teachers.Where(t => 
                    t.FirstName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    t.LastName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    t.Email.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (t.Specialization != null && t.Specialization.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrEmpty(request.Specialization))
            {
                teachers = teachers.Where(t => t.Specialization != null && 
                    t.Specialization.Contains(request.Specialization, StringComparison.OrdinalIgnoreCase));
            }

            if (request.IsActive.HasValue)
            {
                teachers = teachers.Where(t => t.IsActive == request.IsActive.Value);
            }

            if (request.MinHourlyRate.HasValue)
            {
                teachers = teachers.Where(t => t.HourlyRate >= request.MinHourlyRate.Value);
            }

            if (request.MaxHourlyRate.HasValue)
            {
                teachers = teachers.Where(t => t.HourlyRate <= request.MaxHourlyRate.Value);
            }

            // Apply pagination
            var pagedTeachers = teachers
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            var result = pagedTeachers.Select(t => new TeacherListResponse
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber,
                Address = t.Address,
                DateOfBirth = t.DateOfBirth,
                Specialization = t.Specialization,
                Bio = t.Bio,
                HourlyRate = t.HourlyRate,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            });

            _logger.LogInformation("Retrieved {Count} teachers", result.Count());
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all teachers");
            throw;
        }
    }
}
