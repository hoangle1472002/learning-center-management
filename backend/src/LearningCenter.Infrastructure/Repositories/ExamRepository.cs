using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using LearningCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LearningCenter.Infrastructure.Repositories;

public class ExamRepository : IExamRepository
{
    private readonly ApplicationDbContext _context;

    public ExamRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Exam?> GetByIdAsync(int id)
    {
        return await _context.Exams
            .Include(e => e.Class)
            .Include(e => e.ExamResults)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Exam>> GetAllAsync()
    {
        return await _context.Exams
            .Include(e => e.Class)
            .Include(e => e.ExamResults)
            .ToListAsync();
    }

    public async Task<Exam> AddAsync(Exam exam)
    {
        _context.Exams.Add(exam);
        await _context.SaveChangesAsync();
        return exam;
    }

    public async Task<Exam> UpdateAsync(Exam exam)
    {
        _context.Exams.Update(exam);
        await _context.SaveChangesAsync();
        return exam;
    }

    public async Task DeleteAsync(int id)
    {
        var exam = await _context.Exams.FindAsync(id);
        if (exam != null)
        {
            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Exam>> GetExamsByClassIdAsync(int classId)
    {
        return await _context.Exams
            .Include(e => e.Class)
            .Include(e => e.ExamResults)
            .Where(e => e.ClassId == classId)
            .OrderBy(e => e.ExamDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Exam>> GetExamsByStatusAsync(string status)
    {
        return await _context.Exams
            .Include(e => e.Class)
            .Include(e => e.ExamResults)
            .Where(e => e.Status == status)
            .OrderBy(e => e.ExamDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Exam>> GetUpcomingExamsAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _context.Exams
            .Include(e => e.Class)
            .Include(e => e.ExamResults)
            .Where(e => e.ExamDate >= today && e.Status == "Scheduled")
            .OrderBy(e => e.ExamDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Exam>> GetExamsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Exams
            .Include(e => e.Class)
            .Include(e => e.ExamResults)
            .Where(e => e.ExamDate >= startDate && e.ExamDate <= endDate)
            .OrderBy(e => e.ExamDate)
            .ToListAsync();
    }
}
