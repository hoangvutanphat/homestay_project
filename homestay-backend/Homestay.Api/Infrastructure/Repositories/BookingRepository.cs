using Homestay.Api.Domain.Entities;
using Homestay.Api.Domain.Constants;
using Homestay.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Homestay.Api.Infrastructure.Repositories;

public class BookingRepository
{
    private readonly HomestayDbContext _context;

    public BookingRepository(HomestayDbContext context)
    {
        _context = context;
    }

    public async Task<Booking> CreateAsync(Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<Booking?> GetByIdAsync(Guid id)
    {
        return await _context.Bookings
            .AsNoTracking()
            .Include(b => b.Room)
                .ThenInclude(r => r.Homestay)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Booking>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Bookings
            .AsNoTracking()
            .Include(b => b.Room)
                .ThenInclude(r => r.Homestay)
            .Where(b => b.UserId == userId && b.DeletedAt == null)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByHomestayIdAsync(Guid homestayId)
    {
        return await _context.Bookings
            .AsNoTracking()
            .Include(b => b.Room)
                .ThenInclude(r => r.Homestay)
            .Where(b => b.Room.HomestayId == homestayId && b.DeletedAt == null)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> HasConflictingBookingAsync(Guid roomId, DateOnly checkIn, DateOnly checkOut)
    {
        return await _context.Bookings
            .AnyAsync(b => 
                b.RoomId == roomId &&
                b.Status != BookingStatus.Cancelled &&
                b.DeletedAt == null &&
                (
                    (checkIn >= b.CheckIn && checkIn < b.CheckOut) ||
                    (checkOut > b.CheckIn && checkOut <= b.CheckOut) ||
                    (checkIn <= b.CheckIn && checkOut >= b.CheckOut)
                ));
    }

    public async Task<bool> UpdateStatusAsync(Guid id, string status)
    {
        return await _context.Bookings
            .Where(b => b.Id == id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.Status, status)) > 0;
    }

    public async Task<bool> CancelAsync(Guid id, Guid userId)
    {
        var now = DateTime.UtcNow;
        return await _context.Bookings
            .Where(b => b.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.Status, BookingStatus.Cancelled)
                .SetProperty(b => b.DeletedAt, now)
                .SetProperty(b => b.DeletedBy, userId)) > 0;
    }
}
