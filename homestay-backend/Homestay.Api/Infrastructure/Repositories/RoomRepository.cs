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
            .Include(r => r.Homestay)
            .Include(r => r.Amenities)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Room>> GetByHomestayIdAsync(Guid homestayId)
    {
        return await _context.Rooms
            .Include(r => r.Homestay)
            .Include(r => r.Amenities)
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
            .Include(r => r.Amenities)
            .FirstOrDefaultAsync(r => r.Id == roomId);
    }

    public async Task<Room?> GetRoomForUpdateAsync(Guid roomId)
    {
        return await _context.Rooms
            .FirstOrDefaultAsync(r => r.Id == roomId);
    }

    public async Task<bool> UpdateRoomAmenitiesAsync(Guid roomId, List<int> amenityIds)
    {
        var room = await _context.Rooms
            .Include(r => r.Amenities)
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (room == null) return false;

        // Clear existing amenities
        room.Amenities.Clear();

        // Add new amenities
        if (amenityIds != null && amenityIds.Any())
        {
            var amenities = await _context.Amenities
                .Where(a => amenityIds.Contains(a.Id) && a.IsActive == true)
                .ToListAsync();

            // Check if all requested amenities exist
            var invalidIds = amenityIds.Except(amenities.Select(a => a.Id)).ToList();
            if (invalidIds.Any())
            {
                throw new ArgumentException($"Invalid or inactive amenity IDs: {string.Join(", ", invalidIds)}");
            }

            foreach (var amenity in amenities)
            {
                room.Amenities.Add(amenity);
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ValidateAmenityIdsAsync(List<int> amenityIds)
    {
        if (amenityIds == null || !amenityIds.Any()) return true;

        var validCount = await _context.Amenities
            .Where(a => amenityIds.Contains(a.Id) && a.IsActive == true)
            .CountAsync();

        return validCount == amenityIds.Count;
    }

    public async Task<List<Amenity>> GetAllActiveAmenitiesAsync()
    {
        return await _context.Amenities
            .Where(a => a.IsActive == true)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }
}
