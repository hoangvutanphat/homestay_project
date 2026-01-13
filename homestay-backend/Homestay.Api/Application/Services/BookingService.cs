using Homestay.Api.Application.DTOs.Booking;
using Homestay.Api.Application.Interfaces;
using Homestay.Api.Domain.Entities;
using Homestay.Api.Domain.Constants;
using Homestay.Api.Infrastructure.Repositories;

namespace Homestay.Api.Application.Services;

public class BookingService : IBookingService
{
    private readonly BookingRepository _bookingRepository;
    private readonly RoomRepository _roomRepository;
    private readonly RoomAvailabilityRepository _availabilityRepository;

    public BookingService(BookingRepository bookingRepository, RoomRepository roomRepository, RoomAvailabilityRepository availabilityRepository)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _availabilityRepository = availabilityRepository;
    }

    public async Task<BookingResponse> CreateBookingAsync(Guid userId, CreateBookingRequest request)
    {
        ValidateDates(request.CheckIn, request.CheckOut);

        var room = await _roomRepository.GetRoomWithHomestayAsync(request.RoomId);
        if (room == null)
            throw new KeyNotFoundException("Room not found");

        ValidateRoom(room, request.NumberOfGuests);

        var hasConflict = await _bookingRepository.HasConflictingBookingAsync(
            request.RoomId, request.CheckIn, request.CheckOut);
        if (hasConflict)
            throw new InvalidOperationException("Room is already booked for these dates");

        var (numberOfNights, totalPrice) = CalculateBookingCost(request.CheckIn, request.CheckOut, room.BasePrice);

        var booking = new Booking
        {
            UserId = userId,
            RoomId = request.RoomId,
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            Status = BookingStatus.Pending
        };

        var createdBooking = await _bookingRepository.CreateAsync(booking);

        var reserved = await _availabilityRepository.ReserveRoomAsync(
            request.RoomId, request.CheckIn, request.CheckOut, createdBooking.Id);
        
        if (!reserved)
        {
            await _bookingRepository.CancelAsync(createdBooking.Id, userId);
            throw new InvalidOperationException("Failed to reserve room. Please try again.");
        }

        return new BookingResponse
        {
            Id = createdBooking.Id,
            UserId = createdBooking.UserId,
            RoomId = createdBooking.RoomId,
            RoomName = room.RoomName,
            HomestayId = room.HomestayId,
            HomestayName = room.Homestay.Name,
            CheckIn = createdBooking.CheckIn,
            CheckOut = createdBooking.CheckOut,
            NumberOfNights = numberOfNights,
            TotalPrice = totalPrice,
            Status = createdBooking.Status,
            CreatedAt = createdBooking.CreatedAt ?? DateTime.UtcNow
        };
    }

    private static void ValidateDates(DateOnly checkIn, DateOnly checkOut)
    {
        if (checkIn < DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("Check-in date cannot be in the past");

        if (checkIn >= checkOut)
            throw new ArgumentException("Check-out date must be after check-in date");

        var numberOfNights = checkOut.DayNumber - checkIn.DayNumber;
        if (numberOfNights < 1)
            throw new ArgumentException("Minimum booking is 1 night");
    }

    private static void ValidateRoom(Room room, int numberOfGuests)
    {
        if (room.Status != RoomStatus.Active)
            throw new InvalidOperationException($"Room is not available. Status: {room.Status}");

        if (numberOfGuests > room.Capacity)
            throw new ArgumentException($"Room capacity is {room.Capacity} guests");
    }

    private static (int numberOfNights, decimal totalPrice) CalculateBookingCost(DateOnly checkIn, DateOnly checkOut, decimal basePrice)
    {
        var numberOfNights = checkOut.DayNumber - checkIn.DayNumber;
        var totalPrice = basePrice * numberOfNights;
        return (numberOfNights, totalPrice);
    }

    public async Task<IEnumerable<BookingResponse>> GetUserBookingsAsync(Guid userId)
    {
        var bookings = await _bookingRepository.GetByUserIdAsync(userId);
        return bookings.Select(MapToResponse);
    }

    public async Task<IEnumerable<BookingResponse>> GetHomestayBookingsAsync(Guid homestayId)
    {
        var bookings = await _bookingRepository.GetByHomestayIdAsync(homestayId);
        return bookings.Select(MapToResponse);
    }

    public async Task<BookingResponse?> GetBookingByIdAsync(Guid bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        return booking != null ? MapToResponse(booking) : null;
    }

    public async Task<bool> UpdateBookingStatusAsync(Guid bookingId, string status)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null) return false;

        if (!IsValidStatusTransition(booking.Status, status))
            throw new InvalidOperationException($"Cannot change status from {booking.Status} to {status}");

        return await _bookingRepository.UpdateStatusAsync(bookingId, status);
    }

    public async Task<bool> CancelBookingAsync(Guid bookingId, Guid userId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null) return false;

        if (booking.Status == BookingStatus.Completed || booking.Status == BookingStatus.Cancelled)
            throw new InvalidOperationException($"Cannot cancel a {booking.Status} booking");

        var result = await _bookingRepository.CancelAsync(bookingId, userId);
        
        if (result)
        {
            await _availabilityRepository.ReleaseRoomAsync(
                booking.RoomId, booking.CheckIn, booking.CheckOut, bookingId);
        }

        return result;
    }

    private static BookingResponse MapToResponse(Booking booking)
    {
        var (numberOfNights, totalPrice) = CalculateBookingCost(
            booking.CheckIn, 
            booking.CheckOut, 
            booking.Room.BasePrice);

        return new BookingResponse
        {
            Id = booking.Id,
            UserId = booking.UserId,
            RoomId = booking.RoomId,
            RoomName = booking.Room.RoomName,
            HomestayId = booking.Room.HomestayId,
            HomestayName = booking.Room.Homestay.Name,
            CheckIn = booking.CheckIn,
            CheckOut = booking.CheckOut,
            NumberOfNights = numberOfNights,
            TotalPrice = totalPrice,
            Status = booking.Status,
            CreatedAt = booking.CreatedAt ?? DateTime.UtcNow
        };
    }

    private static bool IsValidStatusTransition(string currentStatus, string newStatus)
    {
        return currentStatus switch
        {
            BookingStatus.Pending => newStatus is BookingStatus.Confirmed or BookingStatus.Cancelled,
            BookingStatus.Confirmed => newStatus is BookingStatus.Completed or BookingStatus.Cancelled,
            BookingStatus.Completed => false,
            BookingStatus.Cancelled => false,
            _ => false
        };
    }
}
