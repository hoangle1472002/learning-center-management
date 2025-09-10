using LearningCenter.Application.DTOs.Auth;
using MediatR;

namespace LearningCenter.Application.Commands.Auth;

public class RefreshTokenCommand : IRequest<LoginResponse>
{
    public string RefreshToken { get; set; } = string.Empty;
}
