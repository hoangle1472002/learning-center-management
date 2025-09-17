using LearningCenter.Application.DTOs.Auth;
using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using UserEntity = LearningCenter.Domain.Entities.User;

namespace LearningCenter.Application.Handlers.Auth;

public class SocialLoginCommand : IRequest<LoginResponse>
{
    public SocialLoginRequest Request { get; set; } = null!;
}

public class SocialLoginCommandHandler : IRequestHandler<SocialLoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly ILogger<SocialLoginCommandHandler> _logger;

    public SocialLoginCommandHandler(
        IUserRepository userRepository,
        IJwtService jwtService,
        ILogger<SocialLoginCommandHandler> logger)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<LoginResponse> Handle(SocialLoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var socialRequest = request.Request;
            
            // Validate provider
            if (socialRequest.Provider != "Google" && socialRequest.Provider != "Facebook")
            {
                throw new ArgumentException("Invalid provider. Only Google and Facebook are supported.");
            }

            // For now, we'll create a user with the provided information
            // In a real implementation, you would validate the ID token with the provider
            var user = await _userRepository.GetByEmailAsync(socialRequest.Email ?? "");
            
            if (user == null)
            {
                // Create new user for social login
                user = new UserEntity
                {
                    FirstName = socialRequest.FirstName ?? "Social",
                    LastName = socialRequest.LastName ?? "User",
                    Email = socialRequest.Email ?? "",
                    IsEmailVerified = true, // Social login users are considered verified
                    CreatedAt = DateTime.UtcNow
                };
                
                await _userRepository.AddAsync(user);
            }

            // Generate JWT token
            var token = _jwtService.GenerateAccessToken(user, new List<string>());
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Save refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("Social login successful for user {Email} with provider {Provider}", 
                user.Email, socialRequest.Provider);

            return new LoginResponse
            {
                Token = token,
                AccessToken = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                User = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    DateOfBirth = user.DateOfBirth ?? DateTime.MinValue,
                    IsActive = true, // Default to true for social login
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    Roles = new List<string>() // Will be populated from UserRoles
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during social login for provider {Provider}", request.Request.Provider);
            throw;
        }
    }
}
