using LearningCenter.Domain.Entities;

namespace LearningCenter.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Role> Roles { get; }
    IRepository<UserRole> UserRoles { get; }
    IRepository<Permission> Permissions { get; }
    IRepository<RolePermission> RolePermissions { get; }
    IRepository<RefreshToken> RefreshTokens { get; }
    IRepository<Student> Students { get; }
    IRepository<Teacher> Teachers { get; }
    IRepository<Subject> Subjects { get; }
    IRepository<Class> Classes { get; }
    IRepository<StudentClass> StudentClasses { get; }
    IRepository<ClassSchedule> ClassSchedules { get; }
    IRepository<TeacherSchedule> TeacherSchedules { get; }
    IRepository<Payment> Payments { get; }
    IRepository<Exam> Exams { get; }
    IRepository<ExamResult> ExamResults { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
