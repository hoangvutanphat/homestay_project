using Homestay.Api.Application.DTOs.Room;
using Homestay.Api.Application.Interfaces;
using Homestay.Api.Domain.Entities;
using Homestay.Api.Infrastructure.Repositories;

namespace Homestay.Api.Application.Services;

public class RoomService : IRoomService
{
    private readonly RoomRepository _roomRepository;

    public RoomService(RoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<RoomResponse> CreateRoomAsync(Guid hostId, CreateRoomRequest request)
    {
        var homestayExists = await _roomRepository.HomestayExistsAsync(request.HomestayId);
        if (!homestayExists)
            throw new KeyNotFoundException("Homestay not found");

        var homestayHostId = await _roomRepository.GetHomestayHostIdAsync(request.HomestayId);
        if (homestayHostId == null)
            throw new KeyNotFoundException("Homestay not found");

        if (homestayHostId != hostId)
            throw new UnauthorizedAccessException("You can only create rooms for your own homestay");

        var room = new Room
        {
            HomestayId = request.HomestayId,
            RoomName = request.RoomName,
            BasePrice = request.BasePrice,
            Capacity = request.Capacity,
            Status = request.Status
        };

        var createdRoom = await _roomRepository.CreateAsync(room);
        
        var roomWithHomestay = await _roomRepository.GetByIdAsync(createdRoom.Id);
        if (roomWithHomestay == null)
            throw new InvalidOperationException("Failed to retrieve created room");

        return new RoomResponse
        {
            Id = roomWithHomestay.Id,
            HomestayId = roomWithHomestay.HomestayId,
            HomestayName = roomWithHomestay.Homestay.Name,
            RoomName = roomWithHomestay.RoomName,
            BasePrice = roomWithHomestay.BasePrice,
            Capacity = roomWithHomestay.Capacity,
            Status = roomWithHomestay.Status,
            CreatedAt = roomWithHomestay.CreatedAt ?? DateTime.UtcNow
        };
    }

    public async Task<RoomResponse?> GetRoomByIdAsync(Guid roomId)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        return room != null ? MapToResponse(room) : null;
    }

    public async Task<IEnumerable<RoomResponse>> GetRoomsByHomestayIdAsync(Guid homestayId)
    {
        var rooms = await _roomRepository.GetByHomestayIdAsync(homestayId);
        return rooms.Select(MapToResponse);
    }

    public async Task<bool> UpdateRoomAsync(Guid roomId, Guid hostId, UpdateRoomRequest request)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null) return false;

        var homestayHostId = await _roomRepository.GetHomestayHostIdAsync(room.HomestayId);
        if (homestayHostId != hostId)
            throw new UnauthorizedAccessException("You can only update rooms for your own homestay");

        if (request.RoomName != null) room.RoomName = request.RoomName;
        if (request.BasePrice.HasValue) room.BasePrice = request.BasePrice.Value;
        if (request.Capacity.HasValue) room.Capacity = request.Capacity.Value;
        if (request.Status != null) room.Status = request.Status;

        return await _roomRepository.UpdateAsync(room);
    }

    public async Task<bool> DeleteRoomAsync(Guid roomId, Guid hostId)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null) return false;

        var homestayHostId = await _roomRepository.GetHomestayHostIdAsync(room.HomestayId);
        if (homestayHostId != hostId)
            throw new UnauthorizedAccessException("You can only delete rooms for your own homestay");

        var hasActiveBookings = await _roomRepository.HasActiveBookingsAsync(roomId);
        if (hasActiveBookings)
            throw new InvalidOperationException("Cannot delete room with active bookings (PENDING or CONFIRMED). Please wait for bookings to complete or be cancelled.");

        return await _roomRepository.DeleteAsync(roomId);
    }

    private static RoomResponse MapToResponse(Room room)
    {
        return new RoomResponse
        {
            Id = room.Id,
            HomestayId = room.HomestayId,
            HomestayName = room.Homestay.Name,
            RoomName = room.RoomName,
            BasePrice = room.BasePrice,
            Capacity = room.Capacity,
            Status = room.Status,
            CreatedAt = room.CreatedAt ?? DateTime.UtcNow
        };
    }
}
