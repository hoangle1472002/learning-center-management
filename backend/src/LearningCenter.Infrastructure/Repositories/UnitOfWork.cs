using LearningCenter.Domain.Entities;
using LearningCenter.Domain.Interfaces;
using LearningCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace LearningCenter.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Users = new Repository<User>(_context);
        Roles = new Repository<Role>(_context);
        UserRoles = new Repository<UserRole>(_context);
        Permissions = new Repository<Permission>(_context);
        RolePermissions = new Repository<RolePermission>(_context);
        RefreshTokens = new Repository<RefreshToken>(_context);
        Students = new Repository<Student>(_context);
        Teachers = new Repository<Teacher>(_context);
        Subjects = new Repository<Subject>(_context);
        Classes = new Repository<Class>(_context);
        StudentClasses = new Repository<StudentClass>(_context);
        ClassSchedules = new Repository<ClassSchedule>(_context);
        TeacherSchedules = new Repository<TeacherSchedule>(_context);
        Payments = new Repository<Payment>(_context);
        Exams = new Repository<Exam>(_context);
        ExamResults = new Repository<ExamResult>(_context);
    }

    public IRepository<User> Users { get; }
    public IRepository<Role> Roles { get; }
    public IRepository<UserRole> UserRoles { get; }
    public IRepository<Permission> Permissions { get; }
    public IRepository<RolePermission> RolePermissions { get; }
    public IRepository<RefreshToken> RefreshTokens { get; }
    public IRepository<Student> Students { get; }
    public IRepository<Teacher> Teachers { get; }
    public IRepository<Subject> Subjects { get; }
    public IRepository<Class> Classes { get; }
    public IRepository<StudentClass> StudentClasses { get; }
    public IRepository<ClassSchedule> ClassSchedules { get; }
    public IRepository<TeacherSchedule> TeacherSchedules { get; }
    public IRepository<Payment> Payments { get; }
    public IRepository<Exam> Exams { get; }
    public IRepository<ExamResult> ExamResults { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
