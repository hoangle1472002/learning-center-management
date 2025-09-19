using LearningCenter.Application.DTOs.Class;
using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using ClassEntity = LearningCenter.Domain.Entities.Class;

namespace LearningCenter.Application.Handlers.Class;

public class CreateClassCommand : IRequest<ClassListResponse>
{
    public CreateClassRequest Request { get; set; } = null!;
}

public class CreateClassCommandHandler : IRequestHandler<CreateClassCommand, ClassListResponse>
{
    private readonly IClassRepository _classRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly ILogger<CreateClassCommandHandler> _logger;

    public CreateClassCommandHandler(
        IClassRepository classRepository,
        ISubjectRepository subjectRepository,
        ITeacherRepository teacherRepository,
        ILogger<CreateClassCommandHandler> logger)
    {
        _classRepository = classRepository;
        _subjectRepository = subjectRepository;
        _teacherRepository = teacherRepository;
        _logger = logger;
    }

    public async Task<ClassListResponse> Handle(CreateClassCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating class with name {ClassName}", request.Request.Name);

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

            // Check if class with same code already exists
            if (!string.IsNullOrEmpty(request.Request.Code))
            {
                var existingClass = await _classRepository.GetByCodeAsync(request.Request.Code);
                if (existingClass != null)
                {
                    throw new ArgumentException("Class with this code already exists");
                }
            }

            // Create class
            var classEntity = new ClassEntity
            {
                Name = request.Request.Name,
                Description = request.Request.Description,
                Code = request.Request.Code,
                SubjectId = request.Request.SubjectId,
                TeacherId = request.Request.TeacherId,
                MaxStudents = request.Request.MaxStudents,
                MaxCapacity = request.Request.MaxStudents, // Set alias
                CurrentStudents = 0,
                CurrentEnrollment = 0, // Set alias
                StartDate = request.Request.StartDate,
                EndDate = request.Request.EndDate,
                Price = request.Request.Price,
                Status = request.Request.Status ?? "Draft",
                Room = request.Request.Room,
                Notes = request.Request.Notes,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdClass = await _classRepository.AddAsync(classEntity);

            _logger.LogInformation("Class {ClassId} created successfully", createdClass.Id);

            return new ClassListResponse
            {
                Id = createdClass.Id,
                Name = createdClass.Name,
                Description = createdClass.Description,
                Code = createdClass.Code,
                SubjectName = subject.Name,
                TeacherName = $"{teacher.FirstName} {teacher.LastName}",
                MaxStudents = createdClass.MaxStudents,
                CurrentStudents = createdClass.CurrentStudents,
                StartDate = createdClass.StartDate,
                EndDate = createdClass.EndDate,
                Price = createdClass.Price,
                Status = createdClass.Status,
                Room = createdClass.Room,
                Notes = createdClass.Notes,
                IsActive = createdClass.IsActive,
                CreatedAt = createdClass.CreatedAt,
                UpdatedAt = createdClass.UpdatedAt,
                EnrollmentCount = 0
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating class with name {ClassName}", request.Request.Name);
            throw;
        }
    }
}
