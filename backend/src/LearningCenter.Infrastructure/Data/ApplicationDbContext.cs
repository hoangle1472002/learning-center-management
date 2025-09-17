using LearningCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningCenter.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<StudentClass> StudentClasses { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<ExamResult> ExamResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.ProfileImageUrl).HasMaxLength(500);
            entity.Property(e => e.PasswordHash).IsRequired();
        });

        // Configure Role entity
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
        });

        // Configure UserRole entity
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.RoleId }).IsUnique();
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Permission entity
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Resource).HasMaxLength(100);
            entity.Property(e => e.Action).HasMaxLength(50);
        });

        // Configure RolePermission entity
        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.RoleId, e.PermissionId }).IsUnique();
            
            entity.HasOne(e => e.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure RefreshToken entity
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Token).IsRequired().HasMaxLength(500);
            entity.Property(e => e.RevokedByIp).HasMaxLength(200);
            entity.Property(e => e.ReplacedByToken).HasMaxLength(500);
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Student entity
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.StudentCode).IsUnique();
            entity.Property(e => e.StudentCode).HasMaxLength(20);
            entity.Property(e => e.ParentName).HasMaxLength(200);
            entity.Property(e => e.ParentPhone).HasMaxLength(20);
            // entity.Property(e => e.ParentEmail).HasMaxLength(500); // Removed - not in Student entity
            entity.Property(e => e.Notes).HasMaxLength(500);
            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Teacher entity
        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id);
            // entity.HasIndex(e => e.EmployeeCode).IsUnique(); // Removed - not in Teacher entity
            // entity.Property(e => e.EmployeeCode).HasMaxLength(20); // Removed - not in Teacher entity
            entity.Property(e => e.Specialization).HasMaxLength(200);
            entity.Property(e => e.Bio).HasMaxLength(1000);
            // entity.Property(e => e.Education).HasMaxLength(200); // Removed - not in Teacher entity
            // entity.Property(e => e.Experience).HasMaxLength(200); // Removed - not in Teacher entity
            entity.Property(e => e.HourlyRate).HasColumnType("decimal(10,2)");
            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Subject entity
        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
        });

        // Configure Class entity
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
            
            entity.HasOne(e => e.Subject)
                .WithMany(s => s.Classes)
                .HasForeignKey(e => e.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Teacher)
                .WithMany(t => t.Classes)
                .HasForeignKey(e => e.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure StudentClass entity
        modelBuilder.Entity<StudentClass>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.StudentId, e.ClassId }).IsUnique();
            // entity.Property(e => e.Status).HasMaxLength(50); // Removed - not in StudentClass entity
            // entity.Property(e => e.PaidAmount).HasColumnType("decimal(10,2)"); // Removed - not in StudentClass entity
            // entity.Property(e => e.RemainingAmount).HasColumnType("decimal(10,2)"); // Removed - not in StudentClass entity
            
            entity.HasOne(e => e.Student)
                .WithMany(s => s.StudentClasses)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Class)
                .WithMany(c => c.StudentClasses)
                .HasForeignKey(e => e.ClassId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure ClassSchedule entity
        modelBuilder.Entity<ClassSchedule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Room).HasMaxLength(200);
            entity.Property(e => e.Notes).HasMaxLength(500);
            
            entity.HasOne(e => e.Class)
                .WithMany(c => c.ClassSchedules)
                .HasForeignKey(e => e.ClassId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Schedule entity
        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DayOfWeek).HasMaxLength(50);
            entity.Property(e => e.Room).HasMaxLength(200);
            
            entity.HasOne(e => e.Teacher)
                .WithMany(t => t.Schedules)
                .HasForeignKey(e => e.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Class)
                .WithMany()
                .HasForeignKey(e => e.ClassId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Payment entity
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(10,2)");
            entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TransactionId).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(500);
            
            entity.HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Class)
                .WithMany()
                .HasForeignKey(e => e.ClassId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Exam entity
        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            
            entity.HasOne(e => e.Class)
                .WithMany(c => c.Exams)
                .HasForeignKey(e => e.ClassId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure ExamResult entity
        modelBuilder.Entity<ExamResult>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ExamId, e.StudentId }).IsUnique();
            entity.Property(e => e.Grade).HasMaxLength(10);
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.Property(e => e.Status).HasMaxLength(50);
            
            entity.HasOne(e => e.Exam)
                .WithMany(ex => ex.ExamResults)
                .HasForeignKey(e => e.ExamId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin", Description = "System Administrator" },
            new Role { Id = 2, Name = "Teacher", Description = "Teacher" },
            new Role { Id = 3, Name = "Student", Description = "Student" }
        );

        // Seed Permissions
        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = 1, Name = "Users.Read", Description = "Read users", Resource = "Users", Action = "Read" },
            new Permission { Id = 2, Name = "Users.Write", Description = "Write users", Resource = "Users", Action = "Write" },
            new Permission { Id = 3, Name = "Classes.Read", Description = "Read classes", Resource = "Classes", Action = "Read" },
            new Permission { Id = 4, Name = "Classes.Write", Description = "Write classes", Resource = "Classes", Action = "Write" },
            new Permission { Id = 5, Name = "Students.Read", Description = "Read students", Resource = "Students", Action = "Read" },
            new Permission { Id = 6, Name = "Students.Write", Description = "Write students", Resource = "Students", Action = "Write" }
        );

        // Seed RolePermissions
        modelBuilder.Entity<RolePermission>().HasData(
            // Admin has all permissions
            new RolePermission { Id = 1, RoleId = 1, PermissionId = 1 },
            new RolePermission { Id = 2, RoleId = 1, PermissionId = 2 },
            new RolePermission { Id = 3, RoleId = 1, PermissionId = 3 },
            new RolePermission { Id = 4, RoleId = 1, PermissionId = 4 },
            new RolePermission { Id = 5, RoleId = 1, PermissionId = 5 },
            new RolePermission { Id = 6, RoleId = 1, PermissionId = 6 },
            // Teacher has read permissions
            new RolePermission { Id = 7, RoleId = 2, PermissionId = 3 },
            new RolePermission { Id = 8, RoleId = 2, PermissionId = 5 },
            // Student has read permissions for their own data
            new RolePermission { Id = 9, RoleId = 3, PermissionId = 3 },
            new RolePermission { Id = 10, RoleId = 3, PermissionId = 5 }
        );
    }
}
