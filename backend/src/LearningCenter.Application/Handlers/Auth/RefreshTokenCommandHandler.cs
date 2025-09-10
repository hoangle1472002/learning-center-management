using LearningCenter.Application.Commands.Auth;
using LearningCenter.Application.DTOs.Auth;
using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using LearningCenter.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Auth;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        ILogger<RefreshTokenCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to refresh token");

        var refreshToken = await _unitOfWork.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);
        if (refreshToken == null || !refreshToken.IsActive)
        {
            _logger.LogWarning("Refresh token not found or inactive");
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        var user = await _unitOfWork.Users.GetByIdAsync(refreshToken.UserId);
        if (user == null)
        {
            _logger.LogWarning("User not found for refresh token");
            throw new UnauthorizedAccessException("User not found");
        }

        // Revoke the old refresh token
        refreshToken.IsRevoked = true;
        refreshToken.RevokedAt = DateTime.UtcNow;
        await _unitOfWork.RefreshTokens.UpdateAsync(refreshToken);

        // Get user roles
        var userRoles = await _unitOfWork.UserRoles.FindAsync(ur => ur.UserId == user.Id);
        var roleNames = new List<string>();
        
        foreach (var userRole in userRoles)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(userRole.RoleId);
            if (role != null)
            {
                roleNames.Add(role.Name);
            }
        }

        // Generate new tokens
        var newAccessToken = _jwtService.GenerateAccessToken(user, roleNames);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        // Save new refresh token
        var newRefreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            ReplacedByToken = refreshToken.Token
        };
        await _unitOfWork.RefreshTokens.AddAsync(newRefreshTokenEntity);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Token refreshed successfully for user: {Email}", user.Email);

        return new LoginResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfileImageUrl = user.ProfileImageUrl,
                Roles = roleNames
            }
        };
    }
}
