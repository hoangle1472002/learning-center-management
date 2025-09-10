using LearningCenter.Application.Commands.Auth;
using LearningCenter.Application.DTOs.Auth;
using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using LearningCenter.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Auth;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, LoginResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        ILogger<RegisterCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<LoginResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to register user with email: {Email}", request.Email);

        // Check if user already exists
        var existingUser = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("Registration failed: User already exists with email: {Email}", request.Email);
            throw new ArgumentException("User with this email already exists");
        }

        // Create new user
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender ?? string.Empty,
            PasswordHash = _jwtService.HashPassword(request.Password),
            IsEmailVerified = true, // For demo purposes, auto-verify email
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Assign Student role by default
        var studentRole = await _unitOfWork.Roles.FirstOrDefaultAsync(r => r.Name == "Student");
        if (studentRole != null)
        {
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = studentRole.Id,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.UserRoles.AddAsync(userRole);
        }

        // Create Student profile
        var student = new Student
        {
            UserId = user.Id,
            StudentCode = GenerateStudentCode(),
            EnrollmentDate = DateTime.UtcNow,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        await _unitOfWork.Students.AddAsync(student);

        await _unitOfWork.SaveChangesAsync();

        // Generate tokens
        var accessToken = _jwtService.GenerateAccessToken(user, new List<string> { "Student" });
        var refreshToken = _jwtService.GenerateRefreshToken();

        // Save refresh token
        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
        await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User {Email} registered successfully", request.Email);

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfileImageUrl = user.ProfileImageUrl,
                Roles = new List<string> { "Student" }
            }
        };
    }

    private string GenerateStudentCode()
    {
        var year = DateTime.Now.Year;
        var random = new Random();
        var number = random.Next(1000, 9999);
        return $"ST{year}{number}";
    }
}
