using LearningCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BCrypt.Net;

namespace LearningCenter.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Seed Roles
            await SeedRolesAsync(context, logger);

            // Seed Admin User
            await SeedAdminUserAsync(context, logger);

            // Seed Sample Subjects
            await SeedSubjectsAsync(context, logger);

            // Seed Sample Teachers
            await SeedTeachersAsync(context, logger);

            // Seed Sample Students
            await SeedStudentsAsync(context, logger);

            // Seed Sample Classes
            await SeedClassesAsync(context, logger);

            logger.LogInformation("Database seeded successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private static async Task SeedRolesAsync(ApplicationDbContext context, ILogger logger)
    {
        if (!await context.Roles.AnyAsync())
        {
            var roles = new[]
            {
                new Role { Name = "Admin", Description = "System Administrator" },
                new Role { Name = "Teacher", Description = "Teacher" },
                new Role { Name = "Student", Description = "Student" }
            };

            context.Roles.AddRange(roles);
            await context.SaveChangesAsync();
            logger.LogInformation("Roles seeded");
        }
    }

    private static async Task SeedAdminUserAsync(ApplicationDbContext context, ILogger logger)
    {
        if (!await context.Users.AnyAsync(u => u.Email == "admin@learningcenter.com"))
        {
            var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin");

            var adminUser = new User
            {
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@learningcenter.com",
                PhoneNumber = "+1234567890",
                Address = "123 Admin Street, Admin City",
                DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Gender = "Other",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                IsEmailVerified = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();

            // Add admin role
            var userRole = new UserRole
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id,
                CreatedAt = DateTime.UtcNow
            };

            context.UserRoles.Add(userRole);
            await context.SaveChangesAsync();

            logger.LogInformation("Admin user seeded: admin@learningcenter.com / Admin123!");
        }
    }

    private static async Task SeedSubjectsAsync(ApplicationDbContext context, ILogger logger)
    {
        if (!await context.Subjects.AnyAsync())
        {
            var subjects = new[]
            {
                new Subject
                {
                    Name = "Mathematics",
                    Description = "Basic and advanced mathematics concepts",
                    Code = "MATH001",
                    Price = 100.00m,
                    Duration = 40,
                    Level = "Beginner",
                    Prerequisites = "Basic arithmetic knowledge",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Subject
                {
                    Name = "English Language",
                    Description = "English grammar, vocabulary, and communication skills",
                    Code = "ENG001",
                    Price = 80.00m,
                    Duration = 30,
                    Level = "Beginner",
                    Prerequisites = "Basic English knowledge",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Subject
                {
                    Name = "Computer Science",
                    Description = "Programming fundamentals and computer concepts",
                    Code = "CS001",
                    Price = 150.00m,
                    Duration = 50,
                    Level = "Intermediate",
                    Prerequisites = "Basic computer knowledge",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Subject
                {
                    Name = "Physics",
                    Description = "Fundamental physics concepts and applications",
                    Code = "PHY001",
                    Price = 120.00m,
                    Duration = 45,
                    Level = "Advanced",
                    Prerequisites = "Mathematics knowledge",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Subjects.AddRange(subjects);
            await context.SaveChangesAsync();
            logger.LogInformation("Subjects seeded");
        }
    }

    private static async Task SeedTeachersAsync(ApplicationDbContext context, ILogger logger)
    {
        if (!await context.Teachers.AnyAsync())
        {
            var teacherRole = await context.Roles.FirstAsync(r => r.Name == "Teacher");

            var teachers = new[]
            {
                new Teacher
                {
                    UserId = await CreateUserAsync(context, "John", "Smith", "john.smith@learningcenter.com", "Teacher123!", teacherRole.Id),
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "john.smith@learningcenter.com",
                    PhoneNumber = "+1234567891",
                    Address = "456 Teacher Street, Teacher City",
                    DateOfBirth = new DateTime(1985, 5, 15, 0, 0, 0, DateTimeKind.Utc),
                    Specialization = "Mathematics",
                    Bio = "Experienced mathematics teacher with 10 years of experience",
                    HourlyRate = 25.00m,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Teacher
                {
                    UserId = await CreateUserAsync(context, "Sarah", "Johnson", "sarah.johnson@learningcenter.com", "Teacher123!", teacherRole.Id),
                    FirstName = "Sarah",
                    LastName = "Johnson",
                    Email = "sarah.johnson@learningcenter.com",
                    PhoneNumber = "+1234567892",
                    Address = "789 Teacher Avenue, Teacher City",
                    DateOfBirth = new DateTime(1988, 8, 22, 0, 0, 0, DateTimeKind.Utc),
                    Specialization = "English Language",
                    Bio = "Passionate English teacher specializing in communication skills",
                    HourlyRate = 22.00m,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Teacher
                {
                    UserId = await CreateUserAsync(context, "Mike", "Wilson", "mike.wilson@learningcenter.com", "Teacher123!", teacherRole.Id),
                    FirstName = "Mike",
                    LastName = "Wilson",
                    Email = "mike.wilson@learningcenter.com",
                    PhoneNumber = "+1234567893",
                    Address = "321 Teacher Road, Teacher City",
                    DateOfBirth = new DateTime(1982, 12, 10, 0, 0, 0, DateTimeKind.Utc),
                    Specialization = "Computer Science",
                    Bio = "Software engineer turned teacher with expertise in programming",
                    HourlyRate = 30.00m,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Teachers.AddRange(teachers);
            await context.SaveChangesAsync();
            logger.LogInformation("Teachers seeded");
        }
    }

    private static async Task SeedStudentsAsync(ApplicationDbContext context, ILogger logger)
    {
        if (!await context.Students.AnyAsync())
        {
            var studentRole = await context.Roles.FirstAsync(r => r.Name == "Student");

            var students = new[]
            {
                new Student
                {
                    UserId = await CreateUserAsync(context, "Alice", "Brown", "alice.brown@student.com", "Student123!", studentRole.Id),
                    FirstName = "Alice",
                    LastName = "Brown",
                    Email = "alice.brown@student.com",
                    PhoneNumber = "+1234567894",
                    Address = "654 Student Street, Student City",
                    DateOfBirth = new DateTime(2000, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                    StudentCode = "STU-001",
                    EnrollmentDate = DateTime.UtcNow,
                    ParentName = "Robert Brown",
                    ParentPhone = "+1234567895",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Student
                {
                    UserId = await CreateUserAsync(context, "Bob", "Davis", "bob.davis@student.com", "Student123!", studentRole.Id),
                    FirstName = "Bob",
                    LastName = "Davis",
                    Email = "bob.davis@student.com",
                    PhoneNumber = "+1234567896",
                    Address = "987 Student Avenue, Student City",
                    DateOfBirth = new DateTime(1999, 7, 20, 0, 0, 0, DateTimeKind.Utc),
                    StudentCode = "STU-002",
                    EnrollmentDate = DateTime.UtcNow,
                    ParentName = "Linda Davis",
                    ParentPhone = "+1234567897",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Student
                {
                    UserId = await CreateUserAsync(context, "Carol", "Miller", "carol.miller@student.com", "Student123!", studentRole.Id),
                    FirstName = "Carol",
                    LastName = "Miller",
                    Email = "carol.miller@student.com",
                    PhoneNumber = "+1234567898",
                    Address = "147 Student Road, Student City",
                    DateOfBirth = new DateTime(2001, 11, 8, 0, 0, 0, DateTimeKind.Utc),
                    StudentCode = "STU-003",
                    EnrollmentDate = DateTime.UtcNow,
                    ParentName = "David Miller",
                    ParentPhone = "+1234567899",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Students.AddRange(students);
            await context.SaveChangesAsync();
            logger.LogInformation("Students seeded");
        }
    }

    private static async Task SeedClassesAsync(ApplicationDbContext context, ILogger logger)
    {
        if (!await context.Classes.AnyAsync())
        {
            var mathSubject = await context.Subjects.FirstAsync(s => s.Name == "Mathematics");
            var engSubject = await context.Subjects.FirstAsync(s => s.Name == "English Language");
            var csSubject = await context.Subjects.FirstAsync(s => s.Name == "Computer Science");

            var johnTeacher = await context.Teachers.FirstAsync(t => t.FirstName == "John");
            var sarahTeacher = await context.Teachers.FirstAsync(t => t.FirstName == "Sarah");
            var mikeTeacher = await context.Teachers.FirstAsync(t => t.FirstName == "Mike");

            var classes = new[]
            {
                new Class
                {
                    Name = "Mathematics 101",
                    Description = "Basic mathematics course for beginners",
                    Code = "MATH101",
                    SubjectId = mathSubject.Id,
                    TeacherId = johnTeacher.Id,
                    MaxStudents = 25,
                    MaxCapacity = 25,
                    CurrentStudents = 0,
                    CurrentEnrollment = 0,
                    StartDate = DateTime.UtcNow.AddDays(7),
                    EndDate = DateTime.UtcNow.AddDays(47),
                    Price = 100.00m,
                    Status = "Active",
                    Room = "Room A101",
                    Notes = "Basic mathematics fundamentals",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Class
                {
                    Name = "English Communication",
                    Description = "English speaking and writing skills",
                    Code = "ENG101",
                    SubjectId = engSubject.Id,
                    TeacherId = sarahTeacher.Id,
                    MaxStudents = 20,
                    MaxCapacity = 20,
                    CurrentStudents = 0,
                    CurrentEnrollment = 0,
                    StartDate = DateTime.UtcNow.AddDays(10),
                    EndDate = DateTime.UtcNow.AddDays(40),
                    Price = 80.00m,
                    Status = "Active",
                    Room = "Room B102",
                    Notes = "Focus on communication skills",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Class
                {
                    Name = "Programming Basics",
                    Description = "Introduction to programming concepts",
                    Code = "CS101",
                    SubjectId = csSubject.Id,
                    TeacherId = mikeTeacher.Id,
                    MaxStudents = 15,
                    MaxCapacity = 15,
                    CurrentStudents = 0,
                    CurrentEnrollment = 0,
                    StartDate = DateTime.UtcNow.AddDays(14),
                    EndDate = DateTime.UtcNow.AddDays(64),
                    Price = 150.00m,
                    Status = "Draft",
                    Room = "Computer Lab C103",
                    Notes = "Hands-on programming practice",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Classes.AddRange(classes);
            await context.SaveChangesAsync();
            logger.LogInformation("Classes seeded");
        }
    }

    private static async Task<int> CreateUserAsync(ApplicationDbContext context, string firstName, string lastName, string email, string password, int roleId)
    {
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            IsEmailVerified = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = roleId,
            CreatedAt = DateTime.UtcNow
        };

        context.UserRoles.Add(userRole);
        await context.SaveChangesAsync();

        return user.Id;
    }
}
