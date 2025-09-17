using LearningCenter.Application.DTOs.Student;
using LearningCenter.Application.Handlers.Student;
using LearningCenter.API.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LearningCenter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AuthorizeRole("Admin", "Teacher")] // Admin and Teacher can manage students
public class StudentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<StudentController> _logger;

    public StudentController(IMediator mediator, ILogger<StudentController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all students with pagination and filtering
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentListResponse>>> GetAllStudents(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] int? minAge = null,
        [FromQuery] int? maxAge = null)
    {
        try
        {
            _logger.LogInformation("Getting all students with page {PageNumber}, size {PageSize}", 
                pageNumber, pageSize);

            var query = new GetAllStudentsQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                IsActive = isActive,
                MinAge = minAge,
                MaxAge = maxAge
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all students");
            return StatusCode(500, new { message = "An error occurred while getting students" });
        }
    }

    /// <summary>
    /// Get student by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentListResponse>> GetStudentById(int id)
    {
        try
        {
            _logger.LogInformation("Getting student by id {StudentId}", id);

            var query = new GetStudentByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new { message = "Student not found" });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting student by id {StudentId}", id);
            return StatusCode(500, new { message = "An error occurred while getting student" });
        }
    }

    /// <summary>
    /// Create new student
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<StudentListResponse>> CreateStudent([FromBody] CreateStudentRequest request)
    {
        try
        {
            _logger.LogInformation("Creating student with email {Email}", request.Email);

            var command = new CreateStudentCommand { Request = request };
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetStudentById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Failed to create student: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating student");
            return StatusCode(500, new { message = "An error occurred while creating student" });
        }
    }

    /// <summary>
    /// Update student
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<StudentListResponse>> UpdateStudent(int id, [FromBody] UpdateStudentRequest request)
    {
        try
        {
            _logger.LogInformation("Updating student {StudentId}", id);

            var command = new UpdateStudentCommand 
            { 
                Id = id, 
                Request = request 
            };
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Failed to update student {StudentId}: {Message}", id, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating student {StudentId}", id);
            return StatusCode(500, new { message = "An error occurred while updating student" });
        }
    }

    /// <summary>
    /// Delete student
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStudent(int id)
    {
        try
        {
            _logger.LogInformation("Deleting student {StudentId}", id);

            var command = new DeleteStudentCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting student {StudentId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting student" });
        }
    }

    /// <summary>
    /// Get student's classes
    /// </summary>
    [HttpGet("{id}/classes")]
    public async Task<ActionResult<IEnumerable<StudentClassResponse>>> GetStudentClasses(int id)
    {
        try
        {
            _logger.LogInformation("Getting classes for student {StudentId}", id);

            var query = new GetStudentClassesQuery { StudentId = id };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Failed to get classes for student {StudentId}: {Message}", id, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting classes for student {StudentId}", id);
            return StatusCode(500, new { message = "An error occurred while getting student classes" });
        }
    }
}
