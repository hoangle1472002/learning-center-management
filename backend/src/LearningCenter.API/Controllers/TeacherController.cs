using LearningCenter.Application.DTOs.Teacher;
using LearningCenter.Application.Handlers.Teacher;
using LearningCenter.API.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LearningCenter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AuthorizeRole("Admin")] // Only Admin can manage teachers
public class TeacherController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TeacherController> _logger;

    public TeacherController(IMediator mediator, ILogger<TeacherController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all teachers with pagination and filtering
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeacherListResponse>>> GetAllTeachers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? specialization = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] decimal? minHourlyRate = null,
        [FromQuery] decimal? maxHourlyRate = null)
    {
        try
        {
            _logger.LogInformation("Getting all teachers with page {PageNumber}, size {PageSize}", 
                pageNumber, pageSize);

            var query = new GetAllTeachersQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                Specialization = specialization,
                IsActive = isActive,
                MinHourlyRate = minHourlyRate,
                MaxHourlyRate = maxHourlyRate
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all teachers");
            return StatusCode(500, new { message = "An error occurred while getting teachers" });
        }
    }

    /// <summary>
    /// Get teacher by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TeacherListResponse>> GetTeacherById(int id)
    {
        try
        {
            _logger.LogInformation("Getting teacher by id {TeacherId}", id);

            var query = new GetTeacherByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new { message = "Teacher not found" });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting teacher by id {TeacherId}", id);
            return StatusCode(500, new { message = "An error occurred while getting teacher" });
        }
    }

    /// <summary>
    /// Create new teacher
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TeacherListResponse>> CreateTeacher([FromBody] CreateTeacherRequest request)
    {
        try
        {
            _logger.LogInformation("Creating teacher with email {Email}", request.Email);

            var command = new CreateTeacherCommand { Request = request };
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetTeacherById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Failed to create teacher: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating teacher");
            return StatusCode(500, new { message = "An error occurred while creating teacher" });
        }
    }

    /// <summary>
    /// Update teacher
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<TeacherListResponse>> UpdateTeacher(int id, [FromBody] UpdateTeacherRequest request)
    {
        try
        {
            _logger.LogInformation("Updating teacher {TeacherId}", id);

            var command = new UpdateTeacherCommand 
            { 
                Id = id, 
                Request = request 
            };
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Failed to update teacher {TeacherId}: {Message}", id, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating teacher {TeacherId}", id);
            return StatusCode(500, new { message = "An error occurred while updating teacher" });
        }
    }

    /// <summary>
    /// Delete teacher
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTeacher(int id)
    {
        try
        {
            _logger.LogInformation("Deleting teacher {TeacherId}", id);

            var command = new DeleteTeacherCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting teacher {TeacherId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting teacher" });
        }
    }

    /// <summary>
    /// Get teacher's schedule
    /// </summary>
    [HttpGet("{id}/schedule")]
    public async Task<ActionResult<IEnumerable<TeacherScheduleResponse>>> GetTeacherSchedule(int id)
    {
        try
        {
            _logger.LogInformation("Getting schedule for teacher {TeacherId}", id);

            var query = new GetTeacherScheduleQuery { TeacherId = id };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Failed to get schedule for teacher {TeacherId}: {Message}", id, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting schedule for teacher {TeacherId}", id);
            return StatusCode(500, new { message = "An error occurred while getting teacher schedule" });
        }
    }
}
