using LearningCenter.Domain.Entities;

namespace LearningCenter.Application.Interfaces;

public interface IStudentClassRepository
{
    Task<StudentClass?> GetByIdAsync(int id);
    Task<StudentClass?> GetByStudentAndClassAsync(int studentId, int classId);
    Task<IEnumerable<StudentClass>> GetAllAsync();
    Task<StudentClass> AddAsync(StudentClass studentClass);
    Task<StudentClass> UpdateAsync(StudentClass studentClass);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
