using LearningCenter.Application.DTOs.Exam;
using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using MediatR;

namespace LearningCenter.Application.Handlers.Exam;

public class CreateExamCommand : IRequest<ExamListResponse>
{
    public CreateExamRequest Request { get; set; } = null!;
}

public class CreateExamCommandHandler : IRequestHandler<CreateExamCommand, ExamListResponse>
{
    private readonly IExamRepository _examRepository;
    private readonly IClassRepository _classRepository;

    public CreateExamCommandHandler(
        IExamRepository examRepository,
        IClassRepository classRepository)
    {
        _examRepository = examRepository;
        _classRepository = classRepository;
    }

    public async Task<ExamListResponse> Handle(CreateExamCommand request, CancellationToken cancellationToken)
    {
        // Validate class exists
        var classEntity = await _classRepository.GetByIdAsync(request.Request.ClassId);
        if (classEntity == null)
            throw new ArgumentException("Class not found");

        var exam = new Domain.Entities.Exam
        {
            Title = request.Request.Title,
            Description = request.Request.Description,
            ClassId = request.Request.ClassId,
            ExamDate = request.Request.ExamDate,
            DurationMinutes = request.Request.DurationMinutes,
            TotalMarks = request.Request.TotalMarks,
            PassingMarks = request.Request.PassingMarks,
            ExamType = request.Request.ExamType,
            Instructions = request.Request.Instructions,
            Location = request.Request.Location,
            Status = "Scheduled"
        };

        await _examRepository.AddAsync(exam);
        await _examRepository.SaveChangesAsync();

        return new ExamListResponse
        {
            Id = exam.Id,
            Title = exam.Title,
            Description = exam.Description,
            ClassId = exam.ClassId,
            ClassName = classEntity.Name,
            ExamDate = exam.ExamDate,
            DurationMinutes = exam.DurationMinutes,
            TotalMarks = exam.TotalMarks,
            PassingMarks = exam.PassingMarks,
            ExamType = exam.ExamType,
            Status = exam.Status,
            Instructions = exam.Instructions,
            Location = exam.Location,
            CreatedAt = exam.CreatedAt
        };
    }
}
