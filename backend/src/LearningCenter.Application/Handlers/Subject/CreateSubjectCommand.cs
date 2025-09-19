using LearningCenter.Application.DTOs.Subject;
using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using SubjectEntity = LearningCenter.Domain.Entities.Subject;

namespace LearningCenter.Application.Handlers.Subject;

public class CreateSubjectCommand : IRequest<SubjectListResponse>
{
    public CreateSubjectRequest Request { get; set; } = null!;
}

public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommand, SubjectListResponse>
{
    private readonly ISubjectRepository _subjectRepository;
    private readonly ILogger<CreateSubjectCommandHandler> _logger;

    public CreateSubjectCommandHandler(
        ISubjectRepository subjectRepository,
        ILogger<CreateSubjectCommandHandler> logger)
    {
        _subjectRepository = subjectRepository;
        _logger = logger;
    }

    public async Task<SubjectListResponse> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating subject with name {SubjectName}", request.Request.Name);

            // Check if subject with same code already exists
            if (!string.IsNullOrEmpty(request.Request.Code))
            {
                var existingSubject = await _subjectRepository.GetByCodeAsync(request.Request.Code);
                if (existingSubject != null)
                {
                    throw new ArgumentException("Subject with this code already exists");
                }
            }

            // Create subject
            var subject = new SubjectEntity
            {
                Name = request.Request.Name,
                Description = request.Request.Description,
                Code = request.Request.Code,
                Price = request.Request.Price,
                Duration = request.Request.Duration,
                Level = request.Request.Level,
                Prerequisites = request.Request.Prerequisites,
                IsActive = request.Request.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            var createdSubject = await _subjectRepository.AddAsync(subject);

            _logger.LogInformation("Subject {SubjectId} created successfully", createdSubject.Id);

            return new SubjectListResponse
            {
                Id = createdSubject.Id,
                Name = createdSubject.Name,
                Description = createdSubject.Description,
                Code = createdSubject.Code,
                Price = createdSubject.Price,
                Duration = createdSubject.Duration,
                Level = createdSubject.Level,
                Prerequisites = createdSubject.Prerequisites,
                IsActive = createdSubject.IsActive,
                CreatedAt = createdSubject.CreatedAt,
                UpdatedAt = createdSubject.UpdatedAt,
                ClassCount = 0,
                StudentCount = 0
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating subject with name {SubjectName}", request.Request.Name);
            throw;
        }
    }
}
