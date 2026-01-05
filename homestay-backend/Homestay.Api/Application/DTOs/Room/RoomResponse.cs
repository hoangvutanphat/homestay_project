namespace Homestay.Api.Application.DTOs.Room;

public class RoomResponse
{
    public Guid Id { get; set; }
    public Guid HomestayId { get; set; }
    public string HomestayName { get; set; } = null!;
    public string RoomName { get; set; } = null!;
    public decimal BasePrice { get; set; }
    public int Capacity { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public List<AmenityDto> Amenities { get; set; } = new();
}

public class AmenityDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Icon { get; set; }
}
