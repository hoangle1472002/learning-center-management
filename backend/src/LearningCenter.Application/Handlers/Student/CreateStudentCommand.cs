using LearningCenter.Application.DTOs.Student;
using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using BCrypt.Net;
using UserEntity = LearningCenter.Domain.Entities.User;
using StudentEntity = LearningCenter.Domain.Entities.Student;

namespace LearningCenter.Application.Handlers.Student;

public class CreateStudentCommand : IRequest<StudentListResponse>
{
    public CreateStudentRequest Request { get; set; } = null!;
}

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, StudentListResponse>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<CreateStudentCommandHandler> _logger;

    public CreateStudentCommandHandler(
        IStudentRepository studentRepository,
        IUserRepository userRepository,
        ILogger<CreateStudentCommandHandler> logger)
    {
        _studentRepository = studentRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<StudentListResponse> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating student with email {Email}", request.Request.Email);

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

            // Create student profile
            var student = new StudentEntity
            {
                UserId = createdUser.Id,
                FirstName = request.Request.FirstName,
                LastName = request.Request.LastName,
                Email = request.Request.Email,
                PhoneNumber = request.Request.PhoneNumber,
                Address = request.Request.Address,
                DateOfBirth = request.Request.DateOfBirth,
                ParentName = request.Request.ParentName,
                ParentPhone = request.Request.ParentPhone,
                Notes = request.Request.Notes,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdStudent = await _studentRepository.AddAsync(student);

            _logger.LogInformation("Student {StudentId} created successfully", createdStudent.Id);

            return new StudentListResponse
            {
                Id = createdStudent.Id,
                FirstName = createdStudent.FirstName,
                LastName = createdStudent.LastName,
                Email = createdStudent.Email,
                PhoneNumber = createdStudent.PhoneNumber,
                Address = createdStudent.Address,
                DateOfBirth = createdStudent.DateOfBirth,
                ParentName = createdStudent.ParentName,
                ParentPhone = createdStudent.ParentPhone,
                Notes = createdStudent.Notes,
                IsActive = createdStudent.IsActive,
                CreatedAt = createdStudent.CreatedAt,
                UpdatedAt = createdStudent.UpdatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating student with email {Email}", request.Request.Email);
            throw;
        }
    }
}
