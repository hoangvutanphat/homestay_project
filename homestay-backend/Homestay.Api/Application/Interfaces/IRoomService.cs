using Homestay.Api.Application.DTOs.Room;

namespace Homestay.Api.Application.Interfaces;

public interface IRoomService
{
    Task<RoomResponse> CreateRoomAsync(Guid hostId, CreateRoomRequest request);
    Task<RoomResponse?> GetRoomByIdAsync(Guid roomId);
    Task<IEnumerable<RoomResponse>> GetRoomsByHomestayIdAsync(Guid homestayId);
    Task<bool> UpdateRoomAsync(Guid roomId, Guid hostId, UpdateRoomRequest request);
    Task<bool> DeleteRoomAsync(Guid roomId, Guid hostId);
}
