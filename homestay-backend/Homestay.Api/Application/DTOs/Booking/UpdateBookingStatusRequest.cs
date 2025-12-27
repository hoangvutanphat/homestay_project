using System.ComponentModel.DataAnnotations;
using Homestay.Api.Domain.Constants;

namespace Homestay.Api.Application.DTOs.Booking;

public class UpdateBookingStatusRequest
{
    [Required]
    [RegularExpression($"({BookingStatus.Pending}|{BookingStatus.Confirmed}|{BookingStatus.Completed}|{BookingStatus.Cancelled})", 
        ErrorMessage = "Status must be: PENDING, CONFIRMED, COMPLETED, or CANCELLED")]
    public string Status { get; set; } = null!;
}
