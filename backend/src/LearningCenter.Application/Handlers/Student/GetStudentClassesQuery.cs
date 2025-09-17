using LearningCenter.Application.DTOs.Student;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Student;

public class GetStudentClassesQuery : IRequest<IEnumerable<StudentClassResponse>>
{
    public int StudentId { get; set; }
}

public class GetStudentClassesQueryHandler : IRequestHandler<GetStudentClassesQuery, IEnumerable<StudentClassResponse>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<GetStudentClassesQueryHandler> _logger;

    public GetStudentClassesQueryHandler(
        IStudentRepository studentRepository,
        ILogger<GetStudentClassesQueryHandler> logger)
    {
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<StudentClassResponse>> Handle(GetStudentClassesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting classes for student {StudentId}", request.StudentId);

            var student = await _studentRepository.GetByIdAsync(request.StudentId);
            if (student == null)
            {
                throw new ArgumentException("Student not found");
            }

            var studentClasses = await _studentRepository.GetStudentClassesAsync(request.StudentId);

            var result = studentClasses.Select(sc => new StudentClassResponse
            {
                Id = sc.Id,
                ClassName = sc.Class.Name,
                Subject = sc.Class.Subject.Name,
                TeacherName = $"{sc.Class.Teacher.FirstName} {sc.Class.Teacher.LastName}",
                StartDate = sc.Class.StartDate ?? DateTime.MinValue,
                EndDate = sc.Class.EndDate ?? DateTime.MinValue,
                MaxCapacity = sc.Class.MaxCapacity,
                CurrentEnrollment = sc.Class.CurrentEnrollment,
                IsActive = sc.Class.IsActive,
                EnrollmentDate = sc.EnrollmentDate,
                CompletionDate = sc.CompletionDate ?? DateTime.MinValue
            });

            _logger.LogInformation("Retrieved {Count} classes for student {StudentId}", result.Count(), request.StudentId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting classes for student {StudentId}", request.StudentId);
            throw;
        }
    }
}
