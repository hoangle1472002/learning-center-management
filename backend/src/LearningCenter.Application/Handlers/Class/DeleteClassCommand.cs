using LearningCenter.Application.DTOs.Auth;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Class;

public class DeleteClassCommand : IRequest<ApiResponse>
{
    public int Id { get; set; }
}

public class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, ApiResponse>
{
    private readonly IClassRepository _classRepository;
    private readonly ILogger<DeleteClassCommandHandler> _logger;

    public DeleteClassCommandHandler(
        IClassRepository classRepository,
        ILogger<DeleteClassCommandHandler> logger)
    {
        _classRepository = classRepository;
        _logger = logger;
    }

    public async Task<ApiResponse> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Deleting class {ClassId}", request.Id);

            var classEntity = await _classRepository.GetByIdAsync(request.Id);
            if (classEntity == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Class not found"
                };
            }

            // Check if class has enrolled students
            if (classEntity.StudentClasses != null && classEntity.StudentClasses.Any())
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Cannot delete class that has enrolled students. Please unenroll all students first."
                };
            }

            await _classRepository.DeleteAsync(request.Id);

            _logger.LogInformation("Class {ClassId} deleted successfully", request.Id);

            return new ApiResponse
            {
                Success = true,
                Message = "Class deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting class {ClassId}", request.Id);
            return new ApiResponse
            {
                Success = false,
                Message = "An error occurred while deleting class"
            };
        }
    }
}
