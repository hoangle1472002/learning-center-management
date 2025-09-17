using LearningCenter.Application.DTOs.User;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.User;

public class GetAllUsersQuery : IRequest<IEnumerable<UserListResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? Role { get; set; }
    public bool? IsActive { get; set; }
}

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserListResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetAllUsersQueryHandler> _logger;

    public GetAllUsersQueryHandler(
        IUserRepository userRepository,
        ILogger<GetAllUsersQueryHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<UserListResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting all users with page {PageNumber}, size {PageSize}", 
                request.PageNumber, request.PageSize);

            var users = await _userRepository.GetAllAsync();
            
            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                users = users.Where(u => 
                    u.FirstName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.LastName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (request.IsActive.HasValue)
            {
                users = users.Where(u => u.IsActive == request.IsActive.Value);
            }

            // Apply pagination
            var pagedUsers = users
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            var result = pagedUsers.Select(u => new UserListResponse
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Address = u.Address,
                DateOfBirth = u.DateOfBirth,
                Gender = u.Gender,
                ProfileImageUrl = u.ProfileImageUrl,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            });

            _logger.LogInformation("Retrieved {Count} users", result.Count());
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            throw;
        }
    }
}
