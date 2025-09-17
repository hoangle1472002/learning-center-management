using LearningCenter.Application.DTOs.Auth;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Student;

public class DeleteStudentCommand : IRequest<ApiResponse>
{
    public int Id { get; set; }
}

public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, ApiResponse>
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<DeleteStudentCommandHandler> _logger;

    public DeleteStudentCommandHandler(
        IStudentRepository studentRepository,
        ILogger<DeleteStudentCommandHandler> logger)
    {
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<ApiResponse> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Deleting student {StudentId}", request.Id);

            var student = await _studentRepository.GetByIdAsync(request.Id);
            if (student == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Student not found"
                };
            }

            await _studentRepository.DeleteAsync(request.Id);

            _logger.LogInformation("Student {StudentId} deleted successfully", request.Id);

            return new ApiResponse
            {
                Success = true,
                Message = "Student deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting student {StudentId}", request.Id);
            return new ApiResponse
            {
                Success = false,
                Message = "An error occurred while deleting student"
            };
        }
    }
}
