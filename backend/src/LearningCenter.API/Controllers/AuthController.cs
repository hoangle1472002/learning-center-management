using LearningCenter.Application.Commands.Auth;
using LearningCenter.Application.DTOs.Auth;
using LearningCenter.Application.Handlers.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LearningCenter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            _logger.LogInformation("Login attempt for email: {Email}", request.Email);

            var command = new LoginCommand
            {
                Email = request.Email,
                Password = request.Password,
                RememberMe = request.RememberMe
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Login failed for email: {Email}, reason: {Reason}", request.Email, ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during login for email: {Email}", request.Email);
            return StatusCode(500, new { message = "An unexpected error occurred" });
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            _logger.LogInformation("Registration attempt for email: {Email}", request.Email);

            var command = new RegisterCommand
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Password = request.Password,
                Address = request.Address,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Registration failed for email: {Email}, reason: {Reason}", request.Email, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during registration for email: {Email}", request.Email);
            return StatusCode(500, new { message = "An unexpected error occurred" });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            _logger.LogInformation("Refresh token attempt");

            var command = new RefreshTokenCommand
            {
                RefreshToken = request.RefreshToken
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Refresh token failed, reason: {Reason}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during refresh token");
            return StatusCode(500, new { message = "An unexpected error occurred" });
        }
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout([FromBody] LogoutRequest request)
    {
        try
        {
            _logger.LogInformation("Logout attempt");

            var command = new LogoutCommand
            {
                RefreshToken = request.RefreshToken
            };

            await _mediator.Send(command);
            return Ok(new { message = "Logged out successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during logout");
            return StatusCode(500, new { message = "An unexpected error occurred" });
        }
    }

    [HttpPost("social-login")]
    public async Task<ActionResult<LoginResponse>> SocialLogin([FromBody] SocialLoginRequest request)
    {
        try
        {
            _logger.LogInformation("Social login attempt for provider: {Provider}", request.Provider);

            var command = new SocialLoginCommand
            {
                Request = request
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Social login failed for provider: {Provider}, reason: {Reason}", request.Provider, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during social login for provider: {Provider}", request.Provider);
            return StatusCode(500, new { message = "An unexpected error occurred" });
        }
    }

    [HttpPost("verify-email")]
    public async Task<ActionResult<ApiResponse>> VerifyEmail([FromBody] EmailVerificationRequest request)
    {
        try
        {
            _logger.LogInformation("Email verification attempt for: {Email}", request.Email);

            var command = new VerifyEmailCommand
            {
                Request = request
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during email verification for: {Email}", request.Email);
            return StatusCode(500, new { message = "An unexpected error occurred" });
        }
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        try
        {
            _logger.LogInformation("Forgot password request for: {Email}", request.Email);

            var command = new ForgotPasswordCommand
            {
                Request = request
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during forgot password for: {Email}", request.Email);
            return StatusCode(500, new { message = "An unexpected error occurred" });
        }
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        try
        {
            _logger.LogInformation("Reset password attempt for: {Email}", request.Email);

            var command = new ResetPasswordCommand
            {
                Request = request
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during reset password for: {Email}", request.Email);
            return StatusCode(500, new { message = "An unexpected error occurred" });
        }
    }
}

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class LogoutRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
