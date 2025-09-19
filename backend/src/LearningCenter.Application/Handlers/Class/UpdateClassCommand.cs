using LearningCenter.Application.DTOs.Class;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Class;

public class UpdateClassCommand : IRequest<ClassListResponse>
{
    public int Id { get; set; }
    public UpdateClassRequest Request { get; set; } = null!;
}

public class UpdateClassCommandHandler : IRequestHandler<UpdateClassCommand, ClassListResponse>
{
    private readonly IClassRepository _classRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly ILogger<UpdateClassCommandHandler> _logger;

    public UpdateClassCommandHandler(
        IClassRepository classRepository,
        ISubjectRepository subjectRepository,
        ITeacherRepository teacherRepository,
        ILogger<UpdateClassCommandHandler> logger)
    {
        _classRepository = classRepository;
        _subjectRepository = subjectRepository;
        _teacherRepository = teacherRepository;
        _logger = logger;
    }

    public async Task<ClassListResponse> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Updating class {ClassId}", request.Id);

            var classEntity = await _classRepository.GetByIdAsync(request.Id);
            if (classEntity == null)
            {
                throw new ArgumentException("Class not found");
            }

            // Validate subject exists
            var subject = await _subjectRepository.GetByIdAsync(request.Request.SubjectId);
            if (subject == null)
            {
                throw new ArgumentException("Subject not found");
            }

            // Validate teacher exists
            var teacher = await _teacherRepository.GetByIdAsync(request.Request.TeacherId);
            if (teacher == null)
            {
                throw new ArgumentException("Teacher not found");
            }

            // Check if code is being changed and if it already exists
            if (!string.IsNullOrEmpty(request.Request.Code) && classEntity.Code != request.Request.Code)
            {
                var existingClass = await _classRepository.GetByCodeAsync(request.Request.Code);
                if (existingClass != null)
                {
                    throw new ArgumentException("Class with this code already exists");
                }
            }

            // Update class properties
            classEntity.Name = request.Request.Name;
            classEntity.Description = request.Request.Description;
            classEntity.Code = request.Request.Code;
            classEntity.SubjectId = request.Request.SubjectId;
            classEntity.TeacherId = request.Request.TeacherId;
            classEntity.MaxStudents = request.Request.MaxStudents;
            classEntity.MaxCapacity = request.Request.MaxStudents; // Update alias
            classEntity.StartDate = request.Request.StartDate;
            classEntity.EndDate = request.Request.EndDate;
            classEntity.Price = request.Request.Price;
            classEntity.Status = request.Request.Status ?? "Draft";
            classEntity.Room = request.Request.Room;
            classEntity.Notes = request.Request.Notes;
            classEntity.IsActive = request.Request.IsActive;
            classEntity.UpdatedAt = DateTime.UtcNow;

            var updatedClass = await _classRepository.UpdateAsync(classEntity);

            _logger.LogInformation("Class {ClassId} updated successfully", request.Id);

            return new ClassListResponse
            {
                Id = updatedClass.Id,
                Name = updatedClass.Name,
                Description = updatedClass.Description,
                Code = updatedClass.Code,
                SubjectName = subject.Name,
                TeacherName = $"{teacher.FirstName} {teacher.LastName}",
                MaxStudents = updatedClass.MaxStudents,
                CurrentStudents = updatedClass.CurrentStudents,
                StartDate = updatedClass.StartDate,
                EndDate = updatedClass.EndDate,
                Price = updatedClass.Price,
                Status = updatedClass.Status,
                Room = updatedClass.Room,
                Notes = updatedClass.Notes,
                IsActive = updatedClass.IsActive,
                CreatedAt = updatedClass.CreatedAt,
                UpdatedAt = updatedClass.UpdatedAt,
                EnrollmentCount = updatedClass.StudentClasses?.Count ?? 0
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating class {ClassId}", request.Id);
            throw;
        }
    }
}
