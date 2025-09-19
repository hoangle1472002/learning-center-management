using LearningCenter.Application.Interfaces;
using LearningCenter.Domain.Entities;
using LearningCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LearningCenter.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _context.Payments
            .Include(p => p.Student)
            .Include(p => p.Class)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Payment>> GetAllAsync()
    {
        return await _context.Payments
            .Include(p => p.Student)
            .Include(p => p.Class)
            .ToListAsync();
    }

    public async Task<Payment> AddAsync(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment> UpdateAsync(Payment payment)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task DeleteAsync(int id)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment != null)
        {
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByStudentIdAsync(int studentId)
    {
        return await _context.Payments
            .Include(p => p.Student)
            .Include(p => p.Class)
            .Where(p => p.StudentId == studentId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByClassIdAsync(int classId)
    {
        return await _context.Payments
            .Include(p => p.Student)
            .Include(p => p.Class)
            .Where(p => p.ClassId == classId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetOverduePaymentsAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _context.Payments
            .Include(p => p.Student)
            .Include(p => p.Class)
            .Where(p => p.DueDate < today && p.Status == "Pending")
            .OrderBy(p => p.DueDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status)
    {
        return await _context.Payments
            .Include(p => p.Student)
            .Include(p => p.Class)
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Payments.Where(p => p.Status == "Paid");

        if (startDate.HasValue)
            query = query.Where(p => p.PaidDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(p => p.PaidDate <= endDate.Value);

        return await query.SumAsync(p => p.Amount);
    }

    public async Task<decimal> GetOutstandingAmountAsync()
    {
        return await _context.Payments
            .Where(p => p.Status == "Pending" || p.Status == "Overdue")
            .SumAsync(p => p.Amount);
    }
}
