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

        if (request.AmenityIds != null && request.AmenityIds.Any())
        {
            var isValid = await _roomRepository.ValidateAmenityIdsAsync(request.AmenityIds);
            if (!isValid)
                throw new ArgumentException("One or more amenity IDs are invalid or inactive");
        }

        var room = new Room
        {
            HomestayId = request.HomestayId,
            RoomName = request.RoomName,
            BasePrice = request.BasePrice,
            Capacity = request.Capacity,
            Status = request.Status
        };

        var createdRoom = await _roomRepository.CreateAsync(room);
        
        // Add amenities if provided
        if (request.AmenityIds != null && request.AmenityIds.Any())
        {
            await _roomRepository.UpdateRoomAmenitiesAsync(createdRoom.Id, request.AmenityIds);
        }
        
        var roomWithHomestay = await _roomRepository.GetByIdAsync(createdRoom.Id);
        if (roomWithHomestay == null)
            throw new InvalidOperationException("Failed to retrieve created room");

        return MapToResponse(roomWithHomestay);
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

        if (request.AmenityIds != null && request.AmenityIds.Any())
        {
            var isValid = await _roomRepository.ValidateAmenityIdsAsync(request.AmenityIds);
            if (!isValid)
                throw new ArgumentException("One or more amenity IDs are invalid or inactive");
        }

        var roomToUpdate = await _roomRepository.GetRoomForUpdateAsync(roomId);
        if (roomToUpdate == null) return false;

        if (request.RoomName != null) roomToUpdate.RoomName = request.RoomName;
        if (request.BasePrice.HasValue) roomToUpdate.BasePrice = request.BasePrice.Value;
        if (request.Capacity.HasValue) roomToUpdate.Capacity = request.Capacity.Value;
        if (request.Status != null) roomToUpdate.Status = request.Status;

        var updated = await _roomRepository.UpdateAsync(roomToUpdate);
        
        if (request.AmenityIds != null)
        {
            await _roomRepository.UpdateRoomAmenitiesAsync(roomId, request.AmenityIds);
        }

        return updated;
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
            CreatedAt = room.CreatedAt ?? DateTime.UtcNow,
            Amenities = [.. room.Amenities.Select(a => new AmenityDto
            {
                Id = a.Id,
                Code = a.Code,
                Name = a.Name,
                Icon = a.Icon
            })]
        };
    }

    public async Task<IEnumerable<AmenityDto>> GetAllAmenitiesAsync()
    {
        var amenities = await _roomRepository.GetAllActiveAmenitiesAsync();
        return amenities.Select(a => new AmenityDto
        {
            Id = a.Id,
            Code = a.Code,
            Name = a.Name,
            Icon = a.Icon
        });
    }
}
