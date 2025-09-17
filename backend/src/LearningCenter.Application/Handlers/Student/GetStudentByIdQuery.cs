using LearningCenter.Application.DTOs.Student;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Student;

public class GetStudentByIdQuery : IRequest<StudentListResponse?>
{
    public int Id { get; set; }
}

public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentListResponse?>
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<GetStudentByIdQueryHandler> _logger;

    public GetStudentByIdQueryHandler(
        IStudentRepository studentRepository,
        ILogger<GetStudentByIdQueryHandler> logger)
    {
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<StudentListResponse?> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting student by id {StudentId}", request.Id);

            var student = await _studentRepository.GetByIdAsync(request.Id);
            
            if (student == null)
            {
                _logger.LogWarning("Student with id {StudentId} not found", request.Id);
                return null;
            }

            var result = new StudentListResponse
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                Address = student.Address,
                DateOfBirth = student.DateOfBirth,
                ParentName = student.ParentName,
                ParentPhone = student.ParentPhone,
                Notes = student.Notes,
                IsActive = student.IsActive,
                CreatedAt = student.CreatedAt,
                UpdatedAt = student.UpdatedAt
            };

            _logger.LogInformation("Student {StudentId} retrieved successfully", request.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting student by id {StudentId}", request.Id);
            throw;
        }
    }
}
