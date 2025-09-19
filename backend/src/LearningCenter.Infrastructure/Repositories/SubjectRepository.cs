using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using LearningCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LearningCenter.Infrastructure.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly ApplicationDbContext _context;

    public SubjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Subject?> GetByIdAsync(int id)
    {
        return await _context.Subjects
            .Include(s => s.Classes)
            .ThenInclude(c => c.StudentClasses)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Subject?> GetByCodeAsync(string code)
    {
        return await _context.Subjects
            .Include(s => s.Classes)
            .ThenInclude(c => c.StudentClasses)
            .FirstOrDefaultAsync(s => s.Code == code);
    }

    public async Task<IEnumerable<Subject>> GetAllAsync()
    {
        return await _context.Subjects
            .Include(s => s.Classes)
            .ThenInclude(c => c.StudentClasses)
            .ToListAsync();
    }

    public async Task<Subject> AddAsync(Subject subject)
    {
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
        return subject;
    }

    public async Task<Subject> UpdateAsync(Subject subject)
    {
        _context.Subjects.Update(subject);
        await _context.SaveChangesAsync();
        return subject;
    }

    public async Task DeleteAsync(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject != null)
        {
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Subjects.AnyAsync(s => s.Id == id);
    }
}
