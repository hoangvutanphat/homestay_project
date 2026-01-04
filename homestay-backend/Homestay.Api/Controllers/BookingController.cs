using Homestay.Api.Application.DTOs.Booking;
using Homestay.Api.Application.Interfaces;
using Homestay.Api.Authorization.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Homestay.Api.Controllers;

[ApiController]
[Route("api/bookings")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var booking = await _bookingService.CreateBookingAsync(userId, request);
            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet("my-bookings")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var bookings = await _bookingService.GetUserBookingsAsync(userId);
        return Ok(bookings);
    }

    [HttpGet("homestay/{homestayId}")]
    [Authorize(Policy = AuthorizationPolicies.HostOrAdmin)]
    public async Task<IActionResult> GetHomestayBookings(Guid homestayId)
    {
        var bookings = await _bookingService.GetHomestayBookingsAsync(homestayId);
        return Ok(bookings);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(Guid id)
    {
        var booking = await _bookingService.GetBookingByIdAsync(id);
        if (booking == null)
            return NotFound(new { Message = "Booking not found" });

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        if (booking.UserId != userId && userRole != "ADMIN")
            return Forbid();

        return Ok(booking);
    }

    [HttpPatch("{id}/status")]
    [Authorize(Policy = AuthorizationPolicies.HostOrAdmin)]
    public async Task<IActionResult> UpdateBookingStatus(
        Guid id, 
        [FromBody] UpdateBookingStatusRequest request)
    {
        try
        {
            var success = await _bookingService.UpdateBookingStatusAsync(id, request.Status);
            if (!success)
                return NotFound(new { Message = "Booking not found" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelBooking(Guid id)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _bookingService.CancelBookingAsync(id, userId);
            if (!success)
                return NotFound(new { Message = "Booking not found" });

            return Ok(new { Message = "Booking cancelled successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
