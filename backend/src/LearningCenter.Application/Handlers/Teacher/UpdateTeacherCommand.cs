using LearningCenter.Application.DTOs.Teacher;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Teacher;

public class UpdateTeacherCommand : IRequest<TeacherListResponse>
{
    public int Id { get; set; }
    public UpdateTeacherRequest Request { get; set; } = null!;
}

public class UpdateTeacherCommandHandler : IRequestHandler<UpdateTeacherCommand, TeacherListResponse>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UpdateTeacherCommandHandler> _logger;

    public UpdateTeacherCommandHandler(
        ITeacherRepository teacherRepository,
        IUserRepository userRepository,
        ILogger<UpdateTeacherCommandHandler> logger)
    {
        _teacherRepository = teacherRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<TeacherListResponse> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Updating teacher {TeacherId}", request.Id);

            var teacher = await _teacherRepository.GetByIdAsync(request.Id);
            if (teacher == null)
            {
                throw new ArgumentException("Teacher not found");
            }

            // Check if email is being changed and if it already exists
            if (teacher.Email != request.Request.Email)
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.Request.Email);
                if (existingUser != null && existingUser.Id != teacher.UserId)
                {
                    throw new ArgumentException("User with this email already exists");
                }
            }

            // Update teacher properties
            teacher.FirstName = request.Request.FirstName;
            teacher.LastName = request.Request.LastName;
            teacher.Email = request.Request.Email;
            teacher.PhoneNumber = request.Request.PhoneNumber;
            teacher.Address = request.Request.Address;
            teacher.DateOfBirth = request.Request.DateOfBirth;
            teacher.Specialization = request.Request.Specialization;
            teacher.Bio = request.Request.Bio;
            teacher.HourlyRate = request.Request.HourlyRate;
            teacher.IsActive = request.Request.IsActive;
            teacher.UpdatedAt = DateTime.UtcNow;

            var updatedTeacher = await _teacherRepository.UpdateAsync(teacher);

            _logger.LogInformation("Teacher {TeacherId} updated successfully", request.Id);

            return new TeacherListResponse
            {
                Id = updatedTeacher.Id,
                FirstName = updatedTeacher.FirstName,
                LastName = updatedTeacher.LastName,
                Email = updatedTeacher.Email,
                PhoneNumber = updatedTeacher.PhoneNumber,
                Address = updatedTeacher.Address,
                DateOfBirth = updatedTeacher.DateOfBirth,
                Specialization = updatedTeacher.Specialization,
                Bio = updatedTeacher.Bio,
                HourlyRate = updatedTeacher.HourlyRate,
                IsActive = updatedTeacher.IsActive,
                CreatedAt = updatedTeacher.CreatedAt,
                UpdatedAt = updatedTeacher.UpdatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating teacher {TeacherId}", request.Id);
            throw;
        }
    }
}
