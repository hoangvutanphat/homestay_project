using Homestay.Api.Domain.Entities;
using Homestay.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Homestay.Api.Infrastructure.Repositories;

public class PaymentRepository
{
    private readonly HomestayDbContext _context;

    public PaymentRepository(HomestayDbContext context)
    {
        _context = context;
    }

    public async Task<Payment> CreateAsync(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment?> GetByIdAsync(Guid id)
    {
        return await _context.Payments
            .AsNoTracking()
            .Include(p => p.Booking)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Payment?> GetByBookingIdAsync(Guid bookingId)
    {
        return await _context.Payments
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.BookingId == bookingId);
    }

    public async Task<bool> UpdateAsync(Payment payment)
    {
        _context.Payments.Update(payment);
        return await _context.SaveChangesAsync() > 0;
    }
}
