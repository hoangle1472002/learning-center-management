using LearningCenter.Domain.Entities;

namespace LearningCenter.Application.Interfaces;

public interface IExamRepository
{
    Task<Exam?> GetByIdAsync(int id);
    Task<IEnumerable<Exam>> GetAllAsync();
    Task<Exam> AddAsync(Exam exam);
    Task<Exam> UpdateAsync(Exam exam);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
    Task<IEnumerable<Exam>> GetExamsByClassIdAsync(int classId);
    Task<IEnumerable<Exam>> GetExamsByStatusAsync(string status);
    Task<IEnumerable<Exam>> GetUpcomingExamsAsync();
    Task<IEnumerable<Exam>> GetExamsByDateRangeAsync(DateTime startDate, DateTime endDate);
}
