using LearningCenter.Domain.Entities;

namespace LearningCenter.Application.Interfaces;

public interface ISubjectRepository
{
    Task<Subject?> GetByIdAsync(int id);
    Task<Subject?> GetByCodeAsync(string code);
    Task<IEnumerable<Subject>> GetAllAsync();
    Task<Subject> AddAsync(Subject subject);
    Task<Subject> UpdateAsync(Subject subject);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
