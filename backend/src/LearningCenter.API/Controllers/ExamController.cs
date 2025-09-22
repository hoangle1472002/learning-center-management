using LearningCenter.Application.DTOs.Exam;
using LearningCenter.Application.Handlers.Exam;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningCenter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExamController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExamController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExamListResponse>>> GetExams(
        [FromQuery] string? status = null,
        [FromQuery] int? classId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var query = new GetAllExamsQuery
        {
            Status = status,
            ClassId = classId,
            StartDate = startDate,
            EndDate = endDate
        };

        var exams = await _mediator.Send(query);
        return Ok(exams);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExamListResponse>> GetExam(int id)
    {
        // Implementation for getting single exam
        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<ExamListResponse>> CreateExam(CreateExamRequest request)
    {
        var command = new CreateExamCommand { Request = request };
        var exam = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetExam), new { id = exam.Id }, exam);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<ExamListResponse>> UpdateExam(int id, CreateExamRequest request)
    {
        // Implementation for updating exam
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteExam(int id)
    {
        // Implementation for deleting exam
        return NoContent();
    }

    [HttpGet("class/{classId}")]
    public async Task<ActionResult<IEnumerable<ExamListResponse>>> GetClassExams(int classId)
    {
        var query = new GetAllExamsQuery { ClassId = classId };
        var exams = await _mediator.Send(query);
        return Ok(exams);
    }

    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<ExamListResponse>>> GetUpcomingExams()
    {
        var query = new GetAllExamsQuery { Status = "Scheduled" };
        var exams = await _mediator.Send(query);
        return Ok(exams);
    }
}
