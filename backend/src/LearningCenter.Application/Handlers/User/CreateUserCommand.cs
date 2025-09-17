using LearningCenter.Application.DTOs.User;
using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using BCrypt.Net;
using UserEntity = LearningCenter.Domain.Entities.User;

namespace LearningCenter.Application.Handlers.User;

public class CreateUserCommand : IRequest<UserListResponse>
{
    public CreateUserRequest Request { get; set; } = null!;
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserListResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        ILogger<CreateUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserListResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating user with email {Email}", request.Request.Email);

            // Check if user already exists
            var existingUser = await _userRepository.GetByEmailAsync(request.Request.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("User with this email already exists");
            }

            // Create new user
            var user = new UserEntity
            {
                FirstName = request.Request.FirstName,
                LastName = request.Request.LastName,
                Email = request.Request.Email,
                PhoneNumber = request.Request.PhoneNumber,
                Address = request.Request.Address,
                DateOfBirth = request.Request.DateOfBirth,
                Gender = request.Request.Gender,
                ProfileImageUrl = request.Request.ProfileImageUrl,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Request.Password),
                IsEmailVerified = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.AddAsync(user);

            _logger.LogInformation("User {UserId} created successfully", createdUser.Id);

            return new UserListResponse
            {
                Id = createdUser.Id,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email,
                PhoneNumber = createdUser.PhoneNumber,
                Address = createdUser.Address,
                DateOfBirth = createdUser.DateOfBirth,
                Gender = createdUser.Gender,
                ProfileImageUrl = createdUser.ProfileImageUrl,
                IsActive = createdUser.IsActive,
                CreatedAt = createdUser.CreatedAt,
                UpdatedAt = createdUser.UpdatedAt,
                Roles = new List<string>() // Will be populated when roles are assigned
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user with email {Email}", request.Request.Email);
            throw;
        }
    }
}
