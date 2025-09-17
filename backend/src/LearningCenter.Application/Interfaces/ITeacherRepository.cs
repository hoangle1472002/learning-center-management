using LearningCenter.Domain.Entities;

namespace LearningCenter.Application.Interfaces;

public interface ITeacherRepository
{
    Task<Teacher?> GetByIdAsync(int id);
    Task<IEnumerable<Teacher>> GetAllAsync();
    Task<Teacher> AddAsync(Teacher teacher);
    Task<Teacher> UpdateAsync(Teacher teacher);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Schedule>> GetTeacherSchedulesAsync(int teacherId);
}
