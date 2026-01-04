using Homestay.Api.Application.DTOs.Payment;
using Homestay.Api.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Homestay.Api.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("bookings/{bookingId}/initiate")]
    public async Task<IActionResult> InitiatePayment(Guid bookingId, [FromBody] InitiatePaymentRequest request)
    {
        try
        {
            var payment = await _paymentService.InitiatePaymentAsync(bookingId, request.PaymentMethod);
            return Ok(payment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("bookings/{bookingId}/mock")]
    public async Task<IActionResult> ProcessMockPayment(Guid bookingId, [FromBody] MockPaymentRequest request)
    {
        try
        {
            var payment = await _paymentService.ProcessMockPaymentAsync(bookingId, request.MockStatus);
            return Ok(payment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("bookings/{bookingId}")]
    public async Task<IActionResult> GetPaymentByBookingId(Guid bookingId)
    {
        var payment = await _paymentService.GetPaymentByBookingIdAsync(bookingId);
        return payment != null ? Ok(payment) : NotFound();
    }
}
