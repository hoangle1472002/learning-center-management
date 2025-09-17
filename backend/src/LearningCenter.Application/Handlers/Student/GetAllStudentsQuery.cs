using LearningCenter.Application.DTOs.Student;
using LearningCenter.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LearningCenter.Application.Handlers.Student;

public class GetAllStudentsQuery : IRequest<IEnumerable<StudentListResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public bool? IsActive { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
}

public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, IEnumerable<StudentListResponse>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<GetAllStudentsQueryHandler> _logger;

    public GetAllStudentsQueryHandler(
        IStudentRepository studentRepository,
        ILogger<GetAllStudentsQueryHandler> logger)
    {
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<StudentListResponse>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting all students with page {PageNumber}, size {PageSize}", 
                request.PageNumber, request.PageSize);

            var students = await _studentRepository.GetAllAsync();
            
            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                students = students.Where(s => 
                    s.FirstName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    s.LastName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    s.Email.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (s.ParentName != null && s.ParentName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (request.IsActive.HasValue)
            {
                students = students.Where(s => s.IsActive == request.IsActive.Value);
            }

            if (request.MinAge.HasValue)
            {
                var minBirthDate = DateTime.Now.AddYears(-request.MinAge.Value);
                students = students.Where(s => s.DateOfBirth <= minBirthDate);
            }

            if (request.MaxAge.HasValue)
            {
                var maxBirthDate = DateTime.Now.AddYears(-request.MaxAge.Value);
                students = students.Where(s => s.DateOfBirth >= maxBirthDate);
            }

            // Apply pagination
            var pagedStudents = students
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            var result = pagedStudents.Select(s => new StudentListResponse
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                Address = s.Address,
                DateOfBirth = s.DateOfBirth,
                ParentName = s.ParentName,
                ParentPhone = s.ParentPhone,
                Notes = s.Notes,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            });

            _logger.LogInformation("Retrieved {Count} students", result.Count());
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all students");
            throw;
        }
    }
}
