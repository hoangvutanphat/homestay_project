using Homestay.Api.Application.DTOs;
using Homestay.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Homestay.Api.Application.Interfaces;
public interface IHomestayService
{
    Task<List<HomestayEntity>> GetAllHomestaysAsync();
    Task<HomestayEntity?> GetHomestayByIdAsync(Guid id);
    Task<List<HomestayEntity>> GetAllHomestaysIncludingDeletedAsync();
    Task<List<HomestayEntity>> GetDeletedHomestaysAsync();
    Task<HomestayEntity> CreateHomestayAsync(CreateHomestayRequest request);
    Task<bool> UpdateHomestayAsync(Guid id, UpdateHomestayRequest request);
    Task<bool> SoftDeleteHomestayAsync(Guid id, Guid? deletedBy = null);
    Task<bool> RestoreHomestayAsync(Guid id);
    Task<bool> PermanentlyDeleteHomestayAsync(Guid id);
}