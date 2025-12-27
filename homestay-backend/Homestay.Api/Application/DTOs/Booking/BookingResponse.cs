namespace Homestay.Api.Application.DTOs.Booking;

public class BookingResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public string RoomName { get; set; } = null!;
    public Guid HomestayId { get; set; }
    public string HomestayName { get; set; } = null!;
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public int NumberOfNights { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
