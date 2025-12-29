using System.ComponentModel.DataAnnotations;

namespace Homestay.Api.Application.DTOs.Room;

public class UpdateRoomRequest
{
    [StringLength(100, ErrorMessage = "Room name cannot exceed 100 characters")]
    public string? RoomName { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Base price must be greater than 0")]
    public decimal? BasePrice { get; set; }

    [Range(1, 20, ErrorMessage = "Capacity must be between 1 and 20")]
    public int? Capacity { get; set; }

    [RegularExpression("(ACTIVE|INACTIVE|MAINTENANCE)", ErrorMessage = "Status must be: ACTIVE, INACTIVE, or MAINTENANCE")]
    public string? Status { get; set; }
}
