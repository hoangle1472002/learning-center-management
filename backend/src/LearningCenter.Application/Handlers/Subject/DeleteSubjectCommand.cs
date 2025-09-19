using LearningCenter.Application.DTOs.Auth;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Subject;

public class DeleteSubjectCommand : IRequest<ApiResponse>
{
    public int Id { get; set; }
}

public class DeleteSubjectCommandHandler : IRequestHandler<DeleteSubjectCommand, ApiResponse>
{
    private readonly ISubjectRepository _subjectRepository;
    private readonly ILogger<DeleteSubjectCommandHandler> _logger;

    public DeleteSubjectCommandHandler(
        ISubjectRepository subjectRepository,
        ILogger<DeleteSubjectCommandHandler> logger)
    {
        _subjectRepository = subjectRepository;
        _logger = logger;
    }

    public async Task<ApiResponse> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Deleting subject {SubjectId}", request.Id);

            var subject = await _subjectRepository.GetByIdAsync(request.Id);
            if (subject == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Subject not found"
                };
            }

            // Check if subject has classes
            if (subject.Classes != null && subject.Classes.Any())
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Cannot delete subject that has classes. Please delete all classes first."
                };
            }

            await _subjectRepository.DeleteAsync(request.Id);

            _logger.LogInformation("Subject {SubjectId} deleted successfully", request.Id);

            return new ApiResponse
            {
                Success = true,
                Message = "Subject deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting subject {SubjectId}", request.Id);
            return new ApiResponse
            {
                Success = false,
                Message = "An error occurred while deleting subject"
            };
        }
    }
}
