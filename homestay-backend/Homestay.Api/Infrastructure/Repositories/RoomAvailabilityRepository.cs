using Homestay.Api.Domain.Entities;
using Homestay.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Homestay.Api.Infrastructure.Repositories;

public class RoomAvailabilityRepository
{
    private readonly HomestayDbContext _context;

    public RoomAvailabilityRepository(HomestayDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoomAvailability>> GetRoomAvailabilityAsync(Guid roomId, DateOnly startDate, DateOnly endDate)
    {
        return await _context.RoomAvailabilities
            .AsNoTracking()
            .Where(ra => ra.RoomId == roomId && ra.Date >= startDate && ra.Date <= endDate)
            .OrderBy(ra => ra.Date)
            .ToListAsync();
    }

    public async Task<bool> ReserveRoomAsync(Guid roomId, DateOnly checkIn, DateOnly checkOut, Guid bookingId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        try
        {
            var dates = GenerateDateRange(checkIn, checkOut);
            
            var existingAvailabilities = await _context.RoomAvailabilities
                .Where(ra => ra.RoomId == roomId && dates.Contains(ra.Date))
                .ToListAsync();

            foreach (var date in dates)
            {
                var availability = existingAvailabilities.FirstOrDefault(a => a.Date == date);
                
                if (availability == null)
                {
                    availability = new RoomAvailability
                    {
                        Id = Guid.NewGuid(),
                        RoomId = roomId,
                        Date = date,
                        IsAvailable = false
                    };
                    _context.RoomAvailabilities.Add(availability);
                }
                else if (availability.IsAvailable == false && !availability.IsExpired())
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                availability.Reserve(bookingId);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task ConfirmReservationAsync(Guid roomId, DateOnly checkIn, DateOnly checkOut, Guid bookingId)
    {
        var dates = GenerateDateRange(checkIn, checkOut);
        
        var availabilities = await _context.RoomAvailabilities
            .Where(ra => ra.RoomId == roomId && dates.Contains(ra.Date))
            .ToListAsync();

        foreach (var availability in availabilities.Where(a => a.IsReservedBy(bookingId)))
        {
            availability.Confirm(bookingId);
        }

        await _context.SaveChangesAsync();
    }

    public async Task ReleaseRoomAsync(Guid roomId, DateOnly checkIn, DateOnly checkOut, Guid bookingId)
    {
        var dates = GenerateDateRange(checkIn, checkOut);
        
        var availabilities = await _context.RoomAvailabilities
            .Where(ra => ra.RoomId == roomId && dates.Contains(ra.Date))
            .ToListAsync();

        foreach (var availability in availabilities.Where(a => a.IsReservedBy(bookingId)))
        {
            availability.Release();
        }

        await _context.SaveChangesAsync();
    }

    public async Task ReleaseExpiredReservationsAsync()
    {
        var expiredAvailabilities = await _context.RoomAvailabilities
            .Where(ra => ra.IsAvailable == false && ra.Reason != null && ra.Reason.StartsWith("RESERVED|"))
            .ToListAsync();

        var toRelease = expiredAvailabilities.Where(a => a.IsExpired()).ToList();
        
        foreach (var availability in toRelease)
        {
            availability.Release();
        }

        if (toRelease.Any())
        {
            await _context.SaveChangesAsync();
        }
    }

    private static List<DateOnly> GenerateDateRange(DateOnly start, DateOnly end)
    {
        var dates = new List<DateOnly>();
        for (var date = start; date < end; date = date.AddDays(1))
        {
            dates.Add(date);
        }
        return dates;
    }
}
