using LearningCenter.Application.DTOs.Auth;
using LearningCenter.Application.DTOs.Class;
using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using StudentClassEntity = LearningCenter.Domain.Entities.StudentClass;

namespace LearningCenter.Application.Handlers.Class;

public class EnrollStudentCommand : IRequest<ApiResponse>
{
    public EnrollStudentRequest Request { get; set; } = null!;
}

public class EnrollStudentCommandHandler : IRequestHandler<EnrollStudentCommand, ApiResponse>
{
    private readonly IClassRepository _classRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IStudentClassRepository _studentClassRepository;
    private readonly ILogger<EnrollStudentCommandHandler> _logger;

    public EnrollStudentCommandHandler(
        IClassRepository classRepository,
        IStudentRepository studentRepository,
        IStudentClassRepository studentClassRepository,
        ILogger<EnrollStudentCommandHandler> logger)
    {
        _classRepository = classRepository;
        _studentRepository = studentRepository;
        _studentRepository = studentRepository;
        _studentClassRepository = studentClassRepository;
        _logger = logger;
    }

    public async Task<ApiResponse> Handle(EnrollStudentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Enrolling student {StudentId} in class {ClassId}", 
                request.Request.StudentId, request.Request.ClassId);

            // Validate student exists
            var student = await _studentRepository.GetByIdAsync(request.Request.StudentId);
            if (student == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Student not found"
                };
            }

            // Validate class exists
            var classEntity = await _classRepository.GetByIdAsync(request.Request.ClassId);
            if (classEntity == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Class not found"
                };
            }

            // Check if class is active
            if (!classEntity.IsActive)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Cannot enroll in inactive class"
                };
            }

            // Check if class has available spots
            if (classEntity.CurrentStudents >= classEntity.MaxStudents)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Class is full"
                };
            }

            // Check if student is already enrolled
            var existingEnrollment = await _studentClassRepository.GetByStudentAndClassAsync(
                request.Request.StudentId, request.Request.ClassId);
            if (existingEnrollment != null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Student is already enrolled in this class"
                };
            }

            // Create enrollment
            var enrollment = new StudentClassEntity
            {
                StudentId = request.Request.StudentId,
                ClassId = request.Request.ClassId,
                EnrollmentDate = request.Request.EnrollmentDate ?? DateTime.UtcNow,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _studentClassRepository.AddAsync(enrollment);

            // Update class enrollment count
            classEntity.CurrentStudents++;
            classEntity.CurrentEnrollment = classEntity.CurrentStudents; // Update alias
            await _classRepository.UpdateAsync(classEntity);

            _logger.LogInformation("Student {StudentId} enrolled in class {ClassId} successfully", 
                request.Request.StudentId, request.Request.ClassId);

            return new ApiResponse
            {
                Success = true,
                Message = "Student enrolled successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enrolling student {StudentId} in class {ClassId}", 
                request.Request.StudentId, request.Request.ClassId);
            return new ApiResponse
            {
                Success = false,
                Message = "An error occurred while enrolling student"
            };
        }
    }
}
