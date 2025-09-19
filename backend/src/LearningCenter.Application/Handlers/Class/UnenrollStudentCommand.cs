using LearningCenter.Application.DTOs.Auth;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Class;

public class UnenrollStudentCommand : IRequest<ApiResponse>
{
    public int StudentId { get; set; }
    public int ClassId { get; set; }
}

public class UnenrollStudentCommandHandler : IRequestHandler<UnenrollStudentCommand, ApiResponse>
{
    private readonly IClassRepository _classRepository;
    private readonly IStudentClassRepository _studentClassRepository;
    private readonly ILogger<UnenrollStudentCommandHandler> _logger;

    public UnenrollStudentCommandHandler(
        IClassRepository classRepository,
        IStudentClassRepository studentClassRepository,
        ILogger<UnenrollStudentCommandHandler> logger)
    {
        _classRepository = classRepository;
        _studentClassRepository = studentClassRepository;
        _logger = logger;
    }

    public async Task<ApiResponse> Handle(UnenrollStudentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Unenrolling student {StudentId} from class {ClassId}", 
                request.StudentId, request.ClassId);

            // Find enrollment
            var enrollment = await _studentClassRepository.GetByStudentAndClassAsync(
                request.StudentId, request.ClassId);
            if (enrollment == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Student is not enrolled in this class"
                };
            }

            // Delete enrollment
            await _studentClassRepository.DeleteAsync(enrollment.Id);

            // Update class enrollment count
            var classEntity = await _classRepository.GetByIdAsync(request.ClassId);
            if (classEntity != null)
            {
                classEntity.CurrentStudents = Math.Max(0, classEntity.CurrentStudents - 1);
                classEntity.CurrentEnrollment = classEntity.CurrentStudents; // Update alias
                await _classRepository.UpdateAsync(classEntity);
            }

            _logger.LogInformation("Student {StudentId} unenrolled from class {ClassId} successfully", 
                request.StudentId, request.ClassId);

            return new ApiResponse
            {
                Success = true,
                Message = "Student unenrolled successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unenrolling student {StudentId} from class {ClassId}", 
                request.StudentId, request.ClassId);
            return new ApiResponse
            {
                Success = false,
                Message = "An error occurred while unenrolling student"
            };
        }
    }
}
