using MediatR;

namespace LearningCenter.Application.Commands.Auth;

public class LogoutCommand : IRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
