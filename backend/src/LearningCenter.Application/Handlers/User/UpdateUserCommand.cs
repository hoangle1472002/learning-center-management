using LearningCenter.Application.DTOs.User;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.User;

public class UpdateUserCommand : IRequest<UserListResponse>
{
    public int Id { get; set; }
    public UpdateUserRequest Request { get; set; } = null!;
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserListResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(
        IUserRepository userRepository,
        ILogger<UpdateUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserListResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Updating user {UserId}", request.Id);

            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            // Check if email is being changed and if it already exists
            if (user.Email != request.Request.Email)
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.Request.Email);
                if (existingUser != null && existingUser.Id != request.Id)
                {
                    throw new ArgumentException("User with this email already exists");
                }
            }

            // Update user properties
            user.FirstName = request.Request.FirstName;
            user.LastName = request.Request.LastName;
            user.Email = request.Request.Email;
            user.PhoneNumber = request.Request.PhoneNumber;
            user.Address = request.Request.Address;
            user.DateOfBirth = request.Request.DateOfBirth;
            user.Gender = request.Request.Gender;
            user.ProfileImageUrl = request.Request.ProfileImageUrl;
            user.IsActive = request.Request.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            var updatedUser = await _userRepository.UpdateAsync(user);

            _logger.LogInformation("User {UserId} updated successfully", request.Id);

            return new UserListResponse
            {
                Id = updatedUser.Id,
                FirstName = updatedUser.FirstName,
                LastName = updatedUser.LastName,
                Email = updatedUser.Email,
                PhoneNumber = updatedUser.PhoneNumber,
                Address = updatedUser.Address,
                DateOfBirth = updatedUser.DateOfBirth,
                Gender = updatedUser.Gender,
                ProfileImageUrl = updatedUser.ProfileImageUrl,
                IsActive = updatedUser.IsActive,
                CreatedAt = updatedUser.CreatedAt,
                UpdatedAt = updatedUser.UpdatedAt,
                Roles = updatedUser.UserRoles.Select(ur => ur.Role.Name).ToList()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user {UserId}", request.Id);
            throw;
        }
    }
}
