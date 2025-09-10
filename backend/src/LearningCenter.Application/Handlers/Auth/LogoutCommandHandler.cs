using LearningCenter.Application.Commands.Auth;
using LearningCenter.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Auth;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LogoutCommandHandler> _logger;

    public LogoutCommandHandler(IUnitOfWork unitOfWork, ILogger<LogoutCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to logout");

        var refreshToken = await _unitOfWork.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);
        if (refreshToken != null)
        {
            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            await _unitOfWork.RefreshTokens.UpdateAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();
        }

        _logger.LogInformation("Logout completed");
    }
}
