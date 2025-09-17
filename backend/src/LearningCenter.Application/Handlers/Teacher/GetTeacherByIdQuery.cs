using LearningCenter.Application.DTOs.Teacher;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Teacher;

public class GetTeacherByIdQuery : IRequest<TeacherListResponse?>
{
    public int Id { get; set; }
}

public class GetTeacherByIdQueryHandler : IRequestHandler<GetTeacherByIdQuery, TeacherListResponse?>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly ILogger<GetTeacherByIdQueryHandler> _logger;

    public GetTeacherByIdQueryHandler(
        ITeacherRepository teacherRepository,
        ILogger<GetTeacherByIdQueryHandler> logger)
    {
        _teacherRepository = teacherRepository;
        _logger = logger;
    }

    public async Task<TeacherListResponse?> Handle(GetTeacherByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting teacher by id {TeacherId}", request.Id);

            var teacher = await _teacherRepository.GetByIdAsync(request.Id);
            
            if (teacher == null)
            {
                _logger.LogWarning("Teacher with id {TeacherId} not found", request.Id);
                return null;
            }

            var result = new TeacherListResponse
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Email = teacher.Email,
                PhoneNumber = teacher.PhoneNumber,
                Address = teacher.Address,
                DateOfBirth = teacher.DateOfBirth,
                Specialization = teacher.Specialization,
                Bio = teacher.Bio,
                HourlyRate = teacher.HourlyRate,
                IsActive = teacher.IsActive,
                CreatedAt = teacher.CreatedAt,
                UpdatedAt = teacher.UpdatedAt
            };

            _logger.LogInformation("Teacher {TeacherId} retrieved successfully", request.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting teacher by id {TeacherId}", request.Id);
            throw;
        }
    }
}
