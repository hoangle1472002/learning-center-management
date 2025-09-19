using LearningCenter.Domain.Entities;

namespace LearningCenter.Application.Interfaces;

public interface IClassRepository
{
    Task<Class?> GetByIdAsync(int id);
    Task<Class?> GetByCodeAsync(string code);
    Task<IEnumerable<Class>> GetAllAsync();
    Task<Class> AddAsync(Class classEntity);
    Task<Class> UpdateAsync(Class classEntity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Schedule>> GetClassSchedulesAsync(int classId);
}
