using LearningCenter.Application.DTOs.Auth;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Auth;

public class VerifyEmailCommand : IRequest<ApiResponse>
{
    public EmailVerificationRequest Request { get; set; } = null!;
}

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, ApiResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<VerifyEmailCommandHandler> _logger;

    public VerifyEmailCommandHandler(
        IUserRepository userRepository,
        ILogger<VerifyEmailCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<ApiResponse> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(request.Request.Email);
            
            if (user == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (user.IsEmailVerified)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Email already verified"
                };
            }

            // In a real implementation, you would validate the token
            // For now, we'll just mark the email as verified
            user.IsEmailVerified = true;
            user.UpdatedAt = DateTime.UtcNow;
            
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("Email verified successfully for user {Email}", user.Email);

            return new ApiResponse
            {
                Success = true,
                Message = "Email verified successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying email for {Email}", request.Request.Email);
            return new ApiResponse
            {
                Success = false,
                Message = "An error occurred while verifying email"
            };
        }
    }
}
