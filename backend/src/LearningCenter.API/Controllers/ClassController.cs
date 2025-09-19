using LearningCenter.Application.DTOs.Class;
using LearningCenter.Application.Handlers.Class;
using LearningCenter.API.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LearningCenter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AuthorizeRole("Admin", "Teacher")] // Admin and Teacher can manage classes
public class ClassController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ClassController> _logger;

    public ClassController(IMediator mediator, ILogger<ClassController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all classes with pagination and filtering
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClassListResponse>>> GetAllClasses(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? subjectId = null,
        [FromQuery] int? teacherId = null,
        [FromQuery] string? status = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] DateTime? startDateFrom = null,
        [FromQuery] DateTime? startDateTo = null)
    {
        try
        {
            _logger.LogInformation("Getting all classes with page {PageNumber}, size {PageSize}", 
                pageNumber, pageSize);

            var query = new GetAllClassesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                SubjectId = subjectId,
                TeacherId = teacherId,
                Status = status,
                IsActive = isActive,
                StartDateFrom = startDateFrom,
                StartDateTo = startDateTo
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all classes");
            return StatusCode(500, new { message = "An error occurred while getting classes" });
        }
    }

    /// <summary>
    /// Get class by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ClassListResponse>> GetClassById(int id)
    {
        try
        {
            _logger.LogInformation("Getting class by id {ClassId}", id);

            var query = new GetClassByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new { message = "Class not found" });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting class by id {ClassId}", id);
            return StatusCode(500, new { message = "An error occurred while getting class" });
        }
    }

    /// <summary>
    /// Create new class
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ClassListResponse>> CreateClass([FromBody] CreateClassRequest request)
    {
        try
        {
            _logger.LogInformation("Creating class with name {ClassName}", request.Name);

            var command = new CreateClassCommand { Request = request };
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetClassById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Failed to create class: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating class");
            return StatusCode(500, new { message = "An error occurred while creating class" });
        }
    }

    /// <summary>
    /// Update class
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ClassListResponse>> UpdateClass(int id, [FromBody] UpdateClassRequest request)
    {
        try
        {
            _logger.LogInformation("Updating class {ClassId}", id);

            var command = new UpdateClassCommand 
            { 
                Id = id, 
                Request = request 
            };
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Failed to update class {ClassId}: {Message}", id, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating class {ClassId}", id);
            return StatusCode(500, new { message = "An error occurred while updating class" });
        }
    }

    /// <summary>
    /// Delete class
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteClass(int id)
    {
        try
        {
            _logger.LogInformation("Deleting class {ClassId}", id);

            var command = new DeleteClassCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting class {ClassId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting class" });
        }
    }

    /// <summary>
    /// Enroll student in class
    /// </summary>
    [HttpPost("enroll")]
    public async Task<ActionResult> EnrollStudent([FromBody] EnrollStudentRequest request)
    {
        try
        {
            _logger.LogInformation("Enrolling student {StudentId} in class {ClassId}", 
                request.StudentId, request.ClassId);

            var command = new EnrollStudentCommand { Request = request };
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enrolling student {StudentId} in class {ClassId}", 
                request.StudentId, request.ClassId);
            return StatusCode(500, new { message = "An error occurred while enrolling student" });
        }
    }

    /// <summary>
    /// Unenroll student from class
    /// </summary>
    [HttpPost("unenroll")]
    public async Task<ActionResult> UnenrollStudent([FromBody] UnenrollStudentRequest request)
    {
        try
        {
            _logger.LogInformation("Unenrolling student {StudentId} from class {ClassId}", 
                request.StudentId, request.ClassId);

            var command = new UnenrollStudentCommand 
            { 
                StudentId = request.StudentId, 
                ClassId = request.ClassId 
            };
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unenrolling student {StudentId} from class {ClassId}", 
                request.StudentId, request.ClassId);
            return StatusCode(500, new { message = "An error occurred while unenrolling student" });
        }
    }

    /// <summary>
    /// Get class schedule
    /// </summary>
    [HttpGet("{id}/schedule")]
    public async Task<ActionResult<IEnumerable<ClassScheduleResponse>>> GetClassSchedule(int id)
    {
        try
        {
            _logger.LogInformation("Getting schedule for class {ClassId}", id);

            var query = new GetClassScheduleQuery { ClassId = id };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Failed to get schedule for class {ClassId}: {Message}", id, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting schedule for class {ClassId}", id);
            return StatusCode(500, new { message = "An error occurred while getting class schedule" });
        }
    }
}
