using Homestay.Api.Application.DTOs;
using Homestay.Api.Application.Interfaces;
using Homestay.Api.Infrastructure.Repositories;

namespace Homestay.Api.Application.Services;

public class HomestayService : IHomestayService
{
    private readonly HomestayRepository _repository;
    
    public HomestayService(HomestayRepository repository)
    {
        _repository = repository;
    }
    
    public Task<List<HomestayEntity>> GetAllHomestaysAsync()
    =>  _repository.GetAllHomestaysAsync();
    
    public Task<HomestayEntity?> GetHomestayByIdAsync(Guid id)
        => _repository.GetHomestayByIdAsync(id);

    public Task<List<HomestayEntity>> GetAllHomestaysIncludingDeletedAsync()
        => _repository.GetAllHomestaysIncludingDeletedAsync();

    public Task<List<HomestayEntity>> GetDeletedHomestaysAsync()
        => _repository.GetDeletedHomestaysAsync();
    
    public async Task<HomestayEntity> CreateHomestayAsync(Guid hostId, CreateHomestayRequest request)
    {
        var homestay = new HomestayEntity
        {
            HostId = hostId,
            Name = request.Name,
            Address = request.Address,
            City = request.City,
            Description = request.Description
        };
        
        await _repository.CreateHomestayAsync(homestay);
        return homestay;
    }

    public async Task<bool> UpdateHomestayAsync(Guid id, UpdateHomestayRequest request)
    {
        var homestay = await _repository.GetHomestayByIdAsync(id);
        if (homestay == null) return false;
        
        homestay.Name = request.Name;
        homestay.Address = request.Address;
        homestay.City = request.City;
        homestay.Description = request.Description;
        
        await _repository.UpdateHomestayAsync(id, homestay);
        return true;
    }

    public Task<bool> SoftDeleteHomestayAsync(Guid id, Guid? deletedBy = null)
        => _repository.SoftDeleteHomestayAsync(id, deletedBy);
    
    public Task<bool> RestoreHomestayAsync(Guid id)
        => _repository.RestoreHomestayAsync(id);
    
    public Task<bool> PermanentlyDeleteHomestayAsync(Guid id)
        => _repository.PermanentlyDeleteHomestayAsync(id);
}