using LearningCenter.Application.DTOs.Auth;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using BCrypt.Net;

namespace LearningCenter.Application.Handlers.Auth;

public class ResetPasswordCommand : IRequest<ApiResponse>
{
    public ResetPasswordRequest Request { get; set; } = null!;
}

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ApiResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;

    public ResetPasswordCommandHandler(
        IUserRepository userRepository,
        ILogger<ResetPasswordCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<ApiResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(request.Request.Email);
            
            if (user == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Invalid reset token"
                };
            }

            if (user.ResetPasswordToken != request.Request.Token)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Invalid reset token"
                };
            }

            if (user.ResetPasswordExpiry < DateTime.UtcNow)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Reset token has expired"
                };
            }

            // Hash new password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Request.NewPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;
            
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("Password reset successfully for user {Email}", user.Email);

            return new ApiResponse
            {
                Success = true,
                Message = "Password reset successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password for {Email}", request.Request.Email);
            return new ApiResponse
            {
                Success = false,
                Message = "An error occurred while resetting password"
            };
        }
    }
}
