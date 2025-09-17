using LearningCenter.Application.DTOs.Auth;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Auth;

public class ForgotPasswordCommand : IRequest<ApiResponse>
{
    public ForgotPasswordRequest Request { get; set; } = null!;
}

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ApiResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;

    public ForgotPasswordCommandHandler(
        IUserRepository userRepository,
        ILogger<ForgotPasswordCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<ApiResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(request.Request.Email);
            
            if (user == null)
            {
                // Don't reveal if user exists or not for security
                return new ApiResponse
                {
                    Success = true,
                    Message = "If the email exists, a password reset link has been sent"
                };
            }

            // Generate reset token (in real implementation, use a secure token)
            var resetToken = Guid.NewGuid().ToString();
            user.ResetPasswordToken = resetToken;
            user.ResetPasswordExpiry = DateTime.UtcNow.AddHours(1); // Token expires in 1 hour
            user.UpdatedAt = DateTime.UtcNow;
            
            await _userRepository.UpdateAsync(user);

            // In a real implementation, send email with reset link
            _logger.LogInformation("Password reset token generated for user {Email}", user.Email);

            return new ApiResponse
            {
                Success = true,
                Message = "If the email exists, a password reset link has been sent"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating password reset token for {Email}", request.Request.Email);
            return new ApiResponse
            {
                Success = false,
                Message = "An error occurred while processing your request"
            };
        }
    }
}
