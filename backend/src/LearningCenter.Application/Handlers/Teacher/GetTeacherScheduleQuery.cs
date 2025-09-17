using LearningCenter.Application.DTOs.Teacher;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Teacher;

public class GetTeacherScheduleQuery : IRequest<IEnumerable<TeacherScheduleResponse>>
{
    public int TeacherId { get; set; }
}

public class GetTeacherScheduleQueryHandler : IRequestHandler<GetTeacherScheduleQuery, IEnumerable<TeacherScheduleResponse>>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly ILogger<GetTeacherScheduleQueryHandler> _logger;

    public GetTeacherScheduleQueryHandler(
        ITeacherRepository teacherRepository,
        ILogger<GetTeacherScheduleQueryHandler> logger)
    {
        _teacherRepository = teacherRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<TeacherScheduleResponse>> Handle(GetTeacherScheduleQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting schedule for teacher {TeacherId}", request.TeacherId);

            var teacher = await _teacherRepository.GetByIdAsync(request.TeacherId);
            if (teacher == null)
            {
                throw new ArgumentException("Teacher not found");
            }

            var schedules = await _teacherRepository.GetTeacherSchedulesAsync(request.TeacherId);

            var result = schedules.Select(s => new TeacherScheduleResponse
            {
                Id = s.Id,
                ClassName = s.Class.Name,
                Subject = s.Class.Subject.Name,
                DayOfWeek = s.DayOfWeek,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Room = s.Room,
                MaxCapacity = s.Class.MaxCapacity,
                CurrentEnrollment = s.Class.CurrentEnrollment,
                IsActive = s.IsActive
            });

            _logger.LogInformation("Retrieved {Count} schedule items for teacher {TeacherId}", result.Count(), request.TeacherId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting schedule for teacher {TeacherId}", request.TeacherId);
            throw;
        }
    }
}
