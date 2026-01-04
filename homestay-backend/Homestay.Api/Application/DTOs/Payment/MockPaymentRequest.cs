using System.ComponentModel.DataAnnotations;

namespace Homestay.Api.Application.DTOs.Payment;

public class MockPaymentRequest
{
    [Required]
    [RegularExpression("(SUCCESS|FAILED)", ErrorMessage = "MockStatus must be SUCCESS or FAILED")]
    public string MockStatus { get; set; } = "SUCCESS";
}
