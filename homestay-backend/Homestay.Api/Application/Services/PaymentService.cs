using Homestay.Api.Application.DTOs.Payment;
using Homestay.Api.Application.Interfaces;
using Homestay.Api.Domain.Entities;
using Homestay.Api.Domain.Constants;
using Homestay.Api.Infrastructure.Repositories;

namespace Homestay.Api.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly PaymentRepository _paymentRepository;
    private readonly BookingRepository _bookingRepository;
    private readonly RoomAvailabilityRepository _availabilityRepository;

    public PaymentService(
        PaymentRepository paymentRepository, 
        BookingRepository bookingRepository,
        RoomAvailabilityRepository availabilityRepository)
    {
        _paymentRepository = paymentRepository;
        _bookingRepository = bookingRepository;
        _availabilityRepository = availabilityRepository;
    }

    public async Task<PaymentResponse> InitiatePaymentAsync(Guid bookingId, string paymentMethod)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null)
            throw new KeyNotFoundException("Booking not found");

        if (booking.Status != BookingStatus.Pending)
            throw new InvalidOperationException($"Cannot initiate payment for booking with status {booking.Status}");

        var existingPayment = await _paymentRepository.GetByBookingIdAsync(bookingId);
        if (existingPayment != null)
            throw new InvalidOperationException("Payment already initiated for this booking");

        var numberOfNights = booking.CheckOut.DayNumber - booking.CheckIn.DayNumber;
        var amount = booking.Room.BasePrice * numberOfNights;

        var payment = new Payment
        {
            BookingId = bookingId,
            Amount = amount,
            PaymentMethod = paymentMethod,
            Currency = "VND",
            Status = "PENDING"
        };

        var createdPayment = await _paymentRepository.CreateAsync(payment);

        return MapToResponse(createdPayment);
    }

    public async Task<PaymentResponse> ProcessMockPaymentAsync(Guid bookingId, string mockStatus)
    {
        var payment = await _paymentRepository.GetByBookingIdAsync(bookingId);
        if (payment == null)
            throw new KeyNotFoundException("Payment not found for this booking");

        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null)
            throw new KeyNotFoundException("Booking not found");

        if (mockStatus.ToUpper() == "SUCCESS")
        {
            payment.MarkAsSuccess($"MOCK-TXN-{Guid.NewGuid().ToString()[..8]}");
            await _paymentRepository.UpdateAsync(payment);

            await _bookingRepository.UpdateStatusAsync(bookingId, BookingStatus.Confirmed);

            await _availabilityRepository.ConfirmReservationAsync(
                booking.RoomId, booking.CheckIn, booking.CheckOut, bookingId);
        }
        else
        {
            payment.MarkAsFailed();
            await _paymentRepository.UpdateAsync(payment);
        }

        return MapToResponse(payment);
    }

    public async Task<PaymentResponse?> GetPaymentByBookingIdAsync(Guid bookingId)
    {
        var payment = await _paymentRepository.GetByBookingIdAsync(bookingId);
        return payment != null ? MapToResponse(payment) : null;
    }

    private static PaymentResponse MapToResponse(Payment payment)
    {
        return new PaymentResponse
        {
            PaymentId = payment.Id,
            BookingId = payment.BookingId,
            Amount = payment.Amount,
            Currency = payment.Currency,
            PaymentMethod = payment.PaymentMethod,
            Status = payment.Status,
            PaidAt = payment.PaidAt,
            CreatedAt = payment.CreatedAt ?? DateTime.UtcNow
        };
    }
}
