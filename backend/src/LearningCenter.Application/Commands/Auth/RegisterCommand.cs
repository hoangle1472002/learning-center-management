using LearningCenter.Application.DTOs.Auth;
using MediatR;

namespace LearningCenter.Application.Commands.Auth;

public class RegisterCommand : IRequest<LoginResponse>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Password { get; set; } = string.Empty;
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
}
