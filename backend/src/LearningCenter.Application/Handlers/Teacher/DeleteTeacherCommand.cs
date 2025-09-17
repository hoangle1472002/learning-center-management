using LearningCenter.Application.DTOs.Auth;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Teacher;

public class DeleteTeacherCommand : IRequest<ApiResponse>
{
    public int Id { get; set; }
}

public class DeleteTeacherCommandHandler : IRequestHandler<DeleteTeacherCommand, ApiResponse>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly ILogger<DeleteTeacherCommandHandler> _logger;

    public DeleteTeacherCommandHandler(
        ITeacherRepository teacherRepository,
        ILogger<DeleteTeacherCommandHandler> logger)
    {
        _teacherRepository = teacherRepository;
        _logger = logger;
    }

    public async Task<ApiResponse> Handle(DeleteTeacherCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Deleting teacher {TeacherId}", request.Id);

            var teacher = await _teacherRepository.GetByIdAsync(request.Id);
            if (teacher == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Teacher not found"
                };
            }

            await _teacherRepository.DeleteAsync(request.Id);

            _logger.LogInformation("Teacher {TeacherId} deleted successfully", request.Id);

            return new ApiResponse
            {
                Success = true,
                Message = "Teacher deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting teacher {TeacherId}", request.Id);
            return new ApiResponse
            {
                Success = false,
                Message = "An error occurred while deleting teacher"
            };
        }
    }
}
