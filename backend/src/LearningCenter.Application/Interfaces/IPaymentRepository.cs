using LearningCenter.Domain.Entities;

namespace LearningCenter.Application.Interfaces;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(int id);
    Task<IEnumerable<Payment>> GetAllAsync();
    Task<Payment> AddAsync(Payment payment);
    Task<Payment> UpdateAsync(Payment payment);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
    Task<IEnumerable<Payment>> GetPaymentsByStudentIdAsync(int studentId);
    Task<IEnumerable<Payment>> GetPaymentsByClassIdAsync(int classId);
    Task<IEnumerable<Payment>> GetOverduePaymentsAsync();
    Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status);
    Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<decimal> GetOutstandingAmountAsync();
}
