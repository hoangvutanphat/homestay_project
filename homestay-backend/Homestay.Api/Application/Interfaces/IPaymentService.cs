using Homestay.Api.Application.DTOs.Payment;

namespace Homestay.Api.Application.Interfaces;

public interface IPaymentService
{
    Task<PaymentResponse> InitiatePaymentAsync(Guid bookingId, string paymentMethod);
    Task<PaymentResponse> ProcessMockPaymentAsync(Guid bookingId, string mockStatus);
    Task<PaymentResponse?> GetPaymentByBookingIdAsync(Guid bookingId);
}
