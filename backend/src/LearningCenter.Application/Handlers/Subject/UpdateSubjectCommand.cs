using LearningCenter.Application.DTOs.Subject;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Subject;

public class UpdateSubjectCommand : IRequest<SubjectListResponse>
{
    public int Id { get; set; }
    public UpdateSubjectRequest Request { get; set; } = null!;
}

public class UpdateSubjectCommandHandler : IRequestHandler<UpdateSubjectCommand, SubjectListResponse>
{
    private readonly ISubjectRepository _subjectRepository;
    private readonly ILogger<UpdateSubjectCommandHandler> _logger;

    public UpdateSubjectCommandHandler(
        ISubjectRepository subjectRepository,
        ILogger<UpdateSubjectCommandHandler> logger)
    {
        _subjectRepository = subjectRepository;
        _logger = logger;
    }

    public async Task<SubjectListResponse> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Updating subject {SubjectId}", request.Id);

            var subject = await _subjectRepository.GetByIdAsync(request.Id);
            if (subject == null)
            {
                throw new ArgumentException("Subject not found");
            }

            // Check if code is being changed and if it already exists
            if (!string.IsNullOrEmpty(request.Request.Code) && subject.Code != request.Request.Code)
            {
                var existingSubject = await _subjectRepository.GetByCodeAsync(request.Request.Code);
                if (existingSubject != null)
                {
                    throw new ArgumentException("Subject with this code already exists");
                }
            }

            // Update subject properties
            subject.Name = request.Request.Name;
            subject.Description = request.Request.Description;
            subject.Code = request.Request.Code;
            subject.Price = request.Request.Price;
            subject.Duration = request.Request.Duration;
            subject.Level = request.Request.Level;
            subject.Prerequisites = request.Request.Prerequisites;
            subject.IsActive = request.Request.IsActive;
            subject.UpdatedAt = DateTime.UtcNow;

            var updatedSubject = await _subjectRepository.UpdateAsync(subject);

            _logger.LogInformation("Subject {SubjectId} updated successfully", request.Id);

            return new SubjectListResponse
            {
                Id = updatedSubject.Id,
                Name = updatedSubject.Name,
                Description = updatedSubject.Description,
                Code = updatedSubject.Code,
                Price = updatedSubject.Price,
                Duration = updatedSubject.Duration,
                Level = updatedSubject.Level,
                Prerequisites = updatedSubject.Prerequisites,
                IsActive = updatedSubject.IsActive,
                CreatedAt = updatedSubject.CreatedAt,
                UpdatedAt = updatedSubject.UpdatedAt,
                ClassCount = updatedSubject.Classes?.Count ?? 0,
                StudentCount = updatedSubject.Classes?.SelectMany(c => c.StudentClasses).Count() ?? 0
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating subject {SubjectId}", request.Id);
            throw;
        }
    }
}
