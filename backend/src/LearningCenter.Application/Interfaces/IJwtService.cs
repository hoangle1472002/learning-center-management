using LearningCenter.Domain.Entities;

namespace LearningCenter.Application.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user, List<string> roles);
    string GenerateRefreshToken();
    bool VerifyPassword(string password, string passwordHash);
    string HashPassword(string password);
    bool ValidateToken(string token);
    int GetUserIdFromToken(string token);
}
