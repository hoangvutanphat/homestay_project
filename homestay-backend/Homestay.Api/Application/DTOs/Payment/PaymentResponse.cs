namespace Homestay.Api.Application.DTOs.Payment;

public class PaymentResponse
{
    public Guid PaymentId { get; set; }
    public Guid BookingId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = null!;
    public string PaymentMethod { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
