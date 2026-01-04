using System.ComponentModel.DataAnnotations;

namespace Homestay.Api.Application.DTOs.Payment;

public class InitiatePaymentRequest
{
    [Required]
    public string PaymentMethod { get; set; } = "MOCK";
}
