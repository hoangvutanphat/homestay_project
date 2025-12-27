using System.ComponentModel.DataAnnotations;

namespace Homestay.Api.Application.DTOs.Booking;

public class CreateBookingRequest
{
    [Required(ErrorMessage = "Room ID is required")]
    public Guid RoomId { get; set; }

    [Required(ErrorMessage = "Check-in date is required")]
    public DateOnly CheckIn { get; set; }

    [Required(ErrorMessage = "Check-out date is required")]
    public DateOnly CheckOut { get; set; }

    [Range(1, 20, ErrorMessage = "Number of guests must be between 1 and 20")]
    public int NumberOfGuests { get; set; } = 1;
}
