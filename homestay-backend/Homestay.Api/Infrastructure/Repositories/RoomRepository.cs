using Homestay.Api.Domain.Entities;
using Homestay.Api.Domain.Constants;
using Homestay.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Homestay.Api.Infrastructure.Repositories;

public class RoomRepository
{
    private readonly HomestayDbContext _context;

    public RoomRepository(HomestayDbContext context)
    {
        _context = context;
    }

    public async Task<Room> CreateAsync(Room room)
    {
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task<Room?> GetByIdAsync(Guid id)
    {
        return await _context.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Room>> GetByHomestayIdAsync(Guid homestayId)
    {
        return await _context.Rooms
            .AsNoTracking()
            .Where(r => r.HomestayId == homestayId)
            .OrderBy(r => r.RoomName)
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(Room room)
    {
        _context.Rooms.Update(room);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _context.Rooms
            .Where(r => r.Id == id)
            .ExecuteDeleteAsync() > 0;
    }

    public async Task<bool> HasActiveBookingsAsync(Guid roomId)
    {
        return await _context.Bookings
            .AnyAsync(b => 
                b.RoomId == roomId && 
                (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed) &&
                b.DeletedAt == null);
    }

    public async Task<bool> HomestayExistsAsync(Guid homestayId)
    {
        return await _context.Homestays.AnyAsync(h => h.Id == homestayId && h.DeletedAt == null);
    }

    public async Task<Guid?> GetHomestayHostIdAsync(Guid homestayId)
    {
        return await _context.Homestays
            .AsNoTracking()
            .Where(h => h.Id == homestayId && h.DeletedAt == null)
            .Select(h => h.HostId)
            .FirstOrDefaultAsync();
    }

    public async Task<Room?> GetRoomWithHomestayAsync(Guid roomId)
    {
        return await _context.Rooms
            .AsNoTracking()
            .Include(r => r.Homestay)
            .FirstOrDefaultAsync(r => r.Id == roomId);
    }
}
