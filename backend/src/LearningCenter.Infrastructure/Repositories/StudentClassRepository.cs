using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using LearningCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LearningCenter.Infrastructure.Repositories;

public class StudentClassRepository : IStudentClassRepository
{
    private readonly ApplicationDbContext _context;

    public StudentClassRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentClass?> GetByIdAsync(int id)
    {
        return await _context.StudentClasses
            .Include(sc => sc.Student)
            .Include(sc => sc.Class)
            .ThenInclude(c => c.Subject)
            .Include(sc => sc.Class)
            .ThenInclude(c => c.Teacher)
            .FirstOrDefaultAsync(sc => sc.Id == id);
    }

    public async Task<StudentClass?> GetByStudentAndClassAsync(int studentId, int classId)
    {
        return await _context.StudentClasses
            .Include(sc => sc.Student)
            .Include(sc => sc.Class)
            .ThenInclude(c => c.Subject)
            .Include(sc => sc.Class)
            .ThenInclude(c => c.Teacher)
            .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.ClassId == classId);
    }

    public async Task<IEnumerable<StudentClass>> GetAllAsync()
    {
        return await _context.StudentClasses
            .Include(sc => sc.Student)
            .Include(sc => sc.Class)
            .ThenInclude(c => c.Subject)
            .Include(sc => sc.Class)
            .ThenInclude(c => c.Teacher)
            .ToListAsync();
    }

    public async Task<StudentClass> AddAsync(StudentClass studentClass)
    {
        _context.StudentClasses.Add(studentClass);
        await _context.SaveChangesAsync();
        return studentClass;
    }

    public async Task<StudentClass> UpdateAsync(StudentClass studentClass)
    {
        _context.StudentClasses.Update(studentClass);
        await _context.SaveChangesAsync();
        return studentClass;
    }

    public async Task DeleteAsync(int id)
    {
        var studentClass = await _context.StudentClasses.FindAsync(id);
        if (studentClass != null)
        {
            _context.StudentClasses.Remove(studentClass);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.StudentClasses.AnyAsync(sc => sc.Id == id);
    }
}
