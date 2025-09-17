using LearningCenter.Application.DTOs.Teacher;
using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using BCrypt.Net;
using UserEntity = LearningCenter.Domain.Entities.User;
using TeacherEntity = LearningCenter.Domain.Entities.Teacher;

namespace LearningCenter.Application.Handlers.Teacher;

public class CreateTeacherCommand : IRequest<TeacherListResponse>
{
    public CreateTeacherRequest Request { get; set; } = null!;
}

public class CreateTeacherCommandHandler : IRequestHandler<CreateTeacherCommand, TeacherListResponse>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<CreateTeacherCommandHandler> _logger;

    public CreateTeacherCommandHandler(
        ITeacherRepository teacherRepository,
        IUserRepository userRepository,
        ILogger<CreateTeacherCommandHandler> logger)
    {
        _teacherRepository = teacherRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<TeacherListResponse> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating teacher with email {Email}", request.Request.Email);

            // Check if user already exists
            var existingUser = await _userRepository.GetByEmailAsync(request.Request.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("User with this email already exists");
            }

            // Create user first
            var user = new UserEntity
            {
                FirstName = request.Request.FirstName,
                LastName = request.Request.LastName,
                Email = request.Request.Email,
                PhoneNumber = request.Request.PhoneNumber,
                Address = request.Request.Address,
                DateOfBirth = request.Request.DateOfBirth,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Request.Password),
                IsEmailVerified = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.AddAsync(user);

            // Create teacher profile
            var teacher = new TeacherEntity
            {
                UserId = createdUser.Id,
                FirstName = request.Request.FirstName,
                LastName = request.Request.LastName,
                Email = request.Request.Email,
                PhoneNumber = request.Request.PhoneNumber,
                Address = request.Request.Address,
                DateOfBirth = request.Request.DateOfBirth,
                Specialization = request.Request.Specialization,
                Bio = request.Request.Bio,
                HourlyRate = request.Request.HourlyRate,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdTeacher = await _teacherRepository.AddAsync(teacher);

            _logger.LogInformation("Teacher {TeacherId} created successfully", createdTeacher.Id);

            return new TeacherListResponse
            {
                Id = createdTeacher.Id,
                FirstName = createdTeacher.FirstName,
                LastName = createdTeacher.LastName,
                Email = createdTeacher.Email,
                PhoneNumber = createdTeacher.PhoneNumber,
                Address = createdTeacher.Address,
                DateOfBirth = createdTeacher.DateOfBirth,
                Specialization = createdTeacher.Specialization,
                Bio = createdTeacher.Bio,
                HourlyRate = createdTeacher.HourlyRate,
                IsActive = createdTeacher.IsActive,
                CreatedAt = createdTeacher.CreatedAt,
                UpdatedAt = createdTeacher.UpdatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating teacher with email {Email}", request.Request.Email);
            throw;
        }
    }
}
