using LearningCenter.Application.DTOs.User;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.User;

public class GetUserByIdQuery : IRequest<UserListResponse?>
{
    public int Id { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserListResponse?>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(
        IUserRepository userRepository,
        ILogger<GetUserByIdQueryHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserListResponse?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting user by id {UserId}", request.Id);

            var user = await _userRepository.GetByIdAsync(request.Id);
            
            if (user == null)
            {
                _logger.LogWarning("User with id {UserId} not found", request.Id);
                return null;
            }

            var result = new UserListResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                ProfileImageUrl = user.ProfileImageUrl,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            };

            _logger.LogInformation("User {UserId} retrieved successfully", request.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by id {UserId}", request.Id);
            throw;
        }
    }
}
