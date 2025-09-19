using LearningCenter.Application.DTOs.Class;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Class;

public class GetClassByIdQuery : IRequest<ClassListResponse?>
{
    public int Id { get; set; }
}

public class GetClassByIdQueryHandler : IRequestHandler<GetClassByIdQuery, ClassListResponse?>
{
    private readonly IClassRepository _classRepository;
    private readonly ILogger<GetClassByIdQueryHandler> _logger;

    public GetClassByIdQueryHandler(
        IClassRepository classRepository,
        ILogger<GetClassByIdQueryHandler> logger)
    {
        _classRepository = classRepository;
        _logger = logger;
    }

    public async Task<ClassListResponse?> Handle(GetClassByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting class by id {ClassId}", request.Id);

            var classEntity = await _classRepository.GetByIdAsync(request.Id);
            
            if (classEntity == null)
            {
                _logger.LogWarning("Class with id {ClassId} not found", request.Id);
                return null;
            }

            var result = new ClassListResponse
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                Description = classEntity.Description,
                Code = classEntity.Code,
                SubjectName = classEntity.Subject.Name,
                TeacherName = $"{classEntity.Teacher.FirstName} {classEntity.Teacher.LastName}",
                MaxStudents = classEntity.MaxStudents,
                CurrentStudents = classEntity.CurrentStudents,
                StartDate = classEntity.StartDate,
                EndDate = classEntity.EndDate,
                Price = classEntity.Price,
                Status = classEntity.Status,
                Room = classEntity.Room,
                Notes = classEntity.Notes,
                IsActive = classEntity.IsActive,
                CreatedAt = classEntity.CreatedAt,
                UpdatedAt = classEntity.UpdatedAt,
                EnrollmentCount = classEntity.StudentClasses?.Count ?? 0
            };

            _logger.LogInformation("Class {ClassId} retrieved successfully", request.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting class by id {ClassId}", request.Id);
            throw;
        }
    }
}
