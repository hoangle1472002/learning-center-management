using LearningCenter.Application.DTOs.Class;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Class;

public class GetClassScheduleQuery : IRequest<IEnumerable<ClassScheduleResponse>>
{
    public int ClassId { get; set; }
}

public class GetClassScheduleQueryHandler : IRequestHandler<GetClassScheduleQuery, IEnumerable<ClassScheduleResponse>>
{
    private readonly IClassRepository _classRepository;
    private readonly ILogger<GetClassScheduleQueryHandler> _logger;

    public GetClassScheduleQueryHandler(
        IClassRepository classRepository,
        ILogger<GetClassScheduleQueryHandler> logger)
    {
        _classRepository = classRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ClassScheduleResponse>> Handle(GetClassScheduleQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting schedule for class {ClassId}", request.ClassId);

            var classEntity = await _classRepository.GetByIdAsync(request.ClassId);
            if (classEntity == null)
            {
                throw new ArgumentException("Class not found");
            }

            var schedules = await _classRepository.GetClassSchedulesAsync(request.ClassId);

            var result = schedules.Select(s => new ClassScheduleResponse
            {
                Id = s.Id,
                ClassName = s.Class.Name,
                SubjectName = s.Class.Subject.Name,
                TeacherName = $"{s.Teacher.FirstName} {s.Teacher.LastName}",
                DayOfWeek = s.DayOfWeek,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Room = s.Room,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            });

            _logger.LogInformation("Retrieved {Count} schedule items for class {ClassId}", result.Count(), request.ClassId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting schedule for class {ClassId}", request.ClassId);
            throw;
        }
    }
}
