using LearningCenter.Application.DTOs.Student;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Student;

public class UpdateStudentCommand : IRequest<StudentListResponse>
{
    public int Id { get; set; }
    public UpdateStudentRequest Request { get; set; } = null!;
}

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, StudentListResponse>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UpdateStudentCommandHandler> _logger;

    public UpdateStudentCommandHandler(
        IStudentRepository studentRepository,
        IUserRepository userRepository,
        ILogger<UpdateStudentCommandHandler> logger)
    {
        _studentRepository = studentRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<StudentListResponse> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Updating student {StudentId}", request.Id);

            var student = await _studentRepository.GetByIdAsync(request.Id);
            if (student == null)
            {
                throw new ArgumentException("Student not found");
            }

            // Check if email is being changed and if it already exists
            if (student.Email != request.Request.Email)
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.Request.Email);
                if (existingUser != null && existingUser.Id != student.UserId)
                {
                    throw new ArgumentException("User with this email already exists");
                }
            }

            // Update student properties
            student.FirstName = request.Request.FirstName;
            student.LastName = request.Request.LastName;
            student.Email = request.Request.Email;
            student.PhoneNumber = request.Request.PhoneNumber;
            student.Address = request.Request.Address;
            student.DateOfBirth = request.Request.DateOfBirth;
            student.ParentName = request.Request.ParentName;
            student.ParentPhone = request.Request.ParentPhone;
            student.Notes = request.Request.Notes;
            student.IsActive = request.Request.IsActive;
            student.UpdatedAt = DateTime.UtcNow;

            var updatedStudent = await _studentRepository.UpdateAsync(student);

            _logger.LogInformation("Student {StudentId} updated successfully", request.Id);

            return new StudentListResponse
            {
                Id = updatedStudent.Id,
                FirstName = updatedStudent.FirstName,
                LastName = updatedStudent.LastName,
                Email = updatedStudent.Email,
                PhoneNumber = updatedStudent.PhoneNumber,
                Address = updatedStudent.Address,
                DateOfBirth = updatedStudent.DateOfBirth,
                ParentName = updatedStudent.ParentName,
                ParentPhone = updatedStudent.ParentPhone,
                Notes = updatedStudent.Notes,
                IsActive = updatedStudent.IsActive,
                CreatedAt = updatedStudent.CreatedAt,
                UpdatedAt = updatedStudent.UpdatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating student {StudentId}", request.Id);
            throw;
        }
    }
}
