using Homestay.Api.Domain.Entities;
using Homestay.Api.Infrastructure.Extensions;
using Homestay.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Homestay.Api.Infrastructure.Repositories;

public class HomestayRepository
{
    private readonly HomestayDbContext _context;
    
    public HomestayRepository(HomestayDbContext context)
    {
        _context = context;
    }

    public async Task<List<HomestayEntity>> GetAllHomestaysAsync()
        => await _context.Homestays
            .AsNoTracking()
            .ToListAsync();

    public async Task<HomestayEntity?> GetHomestayByIdAsync(Guid id)
        => await _context.Homestays.FindAsync(id);
    
    public async Task<List<HomestayEntity>> GetAllHomestaysIncludingDeletedAsync()
        => await _context.Homestays
            .IncludeDeleted()
            .AsNoTracking()
            .ToListAsync();
    
    public async Task<List<HomestayEntity>> GetDeletedHomestaysAsync()
        => await _context.Homestays
            .OnlyDeleted()
            .AsNoTracking()
            .ToListAsync();

    public async Task CreateHomestayAsync(HomestayEntity homestay)
    {
        _context.Homestays.Add(homestay);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateHomestayAsync(Guid id, HomestayEntity homestay)
    {
        var existingHomestay = await _context.Homestays.FindAsync(id);
        if (existingHomestay == null)
            throw new KeyNotFoundException("Homestay not found");
        
        existingHomestay.Name = homestay.Name;
        existingHomestay.Address = homestay.Address;
        existingHomestay.City = homestay.City;
        existingHomestay.Description = homestay.Description;
        
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> SoftDeleteHomestayAsync(Guid id, Guid? deletedBy = null)
    {
        var homestay = await GetHomestayIncludingDeletedAsync(id);
        if (homestay == null || homestay.DeletedAt != null)
            return false;

        homestay.DeletedAt = DateTime.Now;
        homestay.DeletedBy = deletedBy;

        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> RestoreHomestayAsync(Guid id)
    {
        var homestay = await GetHomestayIncludingDeletedAsync(id);
        if (homestay == null || homestay.DeletedAt == null) 
            return false;

        homestay.DeletedAt = null;
        homestay.DeletedBy = null;

        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> PermanentlyDeleteHomestayAsync(Guid id)
    {
        var homestay = await GetHomestayIncludingDeletedAsync(id);
        if (homestay == null)
            return false;

        _context.Homestays.Remove(homestay);
        await _context.SaveChangesAsync();
        return true;
    }
    
    private async Task<HomestayEntity?> GetHomestayIncludingDeletedAsync(Guid id)
        => await _context.Homestays
            .IncludeDeleted()
            .FirstOrDefaultAsync(h => h.Id == id);
}