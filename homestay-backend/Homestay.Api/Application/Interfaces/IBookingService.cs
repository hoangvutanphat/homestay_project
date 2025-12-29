using Homestay.Api.Application.DTOs.Booking;

namespace Homestay.Api.Application.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateBookingAsync(Guid userId, CreateBookingRequest request);
    Task<IEnumerable<BookingResponse>> GetUserBookingsAsync(Guid userId);
    Task<IEnumerable<BookingResponse>> GetHomestayBookingsAsync(Guid homestayId);
    Task<BookingResponse?> GetBookingByIdAsync(Guid bookingId);
    Task<bool> UpdateBookingStatusAsync(Guid bookingId, string status);
    Task<bool> CancelBookingAsync(Guid bookingId, Guid userId);
}
