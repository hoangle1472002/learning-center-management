using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using LearningCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LearningCenter.Infrastructure.Repositories;

public class ClassRepository : IClassRepository
{
    private readonly ApplicationDbContext _context;

    public ClassRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Class?> GetByIdAsync(int id)
    {
        return await _context.Classes
            .Include(c => c.Subject)
            .Include(c => c.Teacher)
            .Include(c => c.StudentClasses)
            .ThenInclude(sc => sc.Student)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Class?> GetByCodeAsync(string code)
    {
        return await _context.Classes
            .Include(c => c.Subject)
            .Include(c => c.Teacher)
            .Include(c => c.StudentClasses)
            .ThenInclude(sc => sc.Student)
            .FirstOrDefaultAsync(c => c.Code == code);
    }

    public async Task<IEnumerable<Class>> GetAllAsync()
    {
        return await _context.Classes
            .Include(c => c.Subject)
            .Include(c => c.Teacher)
            .Include(c => c.StudentClasses)
            .ThenInclude(sc => sc.Student)
            .ToListAsync();
    }

    public async Task<Class> AddAsync(Class classEntity)
    {
        _context.Classes.Add(classEntity);
        await _context.SaveChangesAsync();
        return classEntity;
    }

    public async Task<Class> UpdateAsync(Class classEntity)
    {
        _context.Classes.Update(classEntity);
        await _context.SaveChangesAsync();
        return classEntity;
    }

    public async Task DeleteAsync(int id)
    {
        var classEntity = await _context.Classes.FindAsync(id);
        if (classEntity != null)
        {
            _context.Classes.Remove(classEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Classes.AnyAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Schedule>> GetClassSchedulesAsync(int classId)
    {
        return await _context.Schedules
            .Include(s => s.Class)
            .ThenInclude(c => c.Subject)
            .Include(s => s.Teacher)
            .Where(s => s.ClassId == classId)
            .ToListAsync();
    }
}
