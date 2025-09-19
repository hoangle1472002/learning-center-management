using LearningCenter.Application.DTOs.Subject;
using LearningCenter.Application.Handlers.Subject;
using LearningCenter.API.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LearningCenter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AuthorizeRole("Admin", "Teacher")] // Admin and Teacher can manage subjects
public class SubjectController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SubjectController> _logger;

    public SubjectController(IMediator mediator, ILogger<SubjectController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all subjects with pagination and filtering
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubjectListResponse>>> GetAllSubjects(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? level = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null)
    {
        try
        {
            _logger.LogInformation("Getting all subjects with page {PageNumber}, size {PageSize}", 
                pageNumber, pageSize);

            var query = new GetAllSubjectsQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                Level = level,
                IsActive = isActive,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all subjects");
            return StatusCode(500, new { message = "An error occurred while getting subjects" });
        }
    }

    /// <summary>
    /// Get subject by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<SubjectListResponse>> GetSubjectById(int id)
    {
        try
        {
            _logger.LogInformation("Getting subject by id {SubjectId}", id);

            var query = new GetSubjectByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new { message = "Subject not found" });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting subject by id {SubjectId}", id);
            return StatusCode(500, new { message = "An error occurred while getting subject" });
        }
    }

    /// <summary>
    /// Create new subject
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<SubjectListResponse>> CreateSubject([FromBody] CreateSubjectRequest request)
    {
        try
        {
            _logger.LogInformation("Creating subject with name {SubjectName}", request.Name);

            var command = new CreateSubjectCommand { Request = request };
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetSubjectById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Failed to create subject: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating subject");
            return StatusCode(500, new { message = "An error occurred while creating subject" });
        }
    }

    /// <summary>
    /// Update subject
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<SubjectListResponse>> UpdateSubject(int id, [FromBody] UpdateSubjectRequest request)
    {
        try
        {
            _logger.LogInformation("Updating subject {SubjectId}", id);

            var command = new UpdateSubjectCommand 
            { 
                Id = id, 
                Request = request 
            };
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Failed to update subject {SubjectId}: {Message}", id, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating subject {SubjectId}", id);
            return StatusCode(500, new { message = "An error occurred while updating subject" });
        }
    }

    /// <summary>
    /// Delete subject
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSubject(int id)
    {
        try
        {
            _logger.LogInformation("Deleting subject {SubjectId}", id);

            var command = new DeleteSubjectCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting subject {SubjectId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting subject" });
        }
    }
}
