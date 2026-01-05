using Homestay.Api.Application.DTOs.Room;
using Homestay.Api.Application.Interfaces;
using Homestay.Api.Authorization.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Homestay.Api.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.HostOrAdmin)]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest request)
    {
        try
        {
            var hostId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var room = await _roomService.CreateRoomAsync(hostId, request);
            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException )
        {
            return Forbid();
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoom(Guid id)
    {
        var room = await _roomService.GetRoomByIdAsync(id);
        if (room == null)
            return NotFound(new { Message = "Room not found" });

        return Ok(room);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.HostOrAdmin)]
    public async Task<IActionResult> UpdateRoom(Guid id, [FromBody] UpdateRoomRequest request)
    {
        try
        {
            var hostId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _roomService.UpdateRoomAsync(id, hostId, request);
            
            if (!success)
                return NotFound(new { Message = "Room not found" });

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException )
        {
            return Forbid();
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.HostOrAdmin)]
    public async Task<IActionResult> DeleteRoom(Guid id)
    {
        try
        {
            var hostId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _roomService.DeleteRoomAsync(id, hostId);
            
            if (!success)
                return NotFound(new { Message = "Room not found" });

            return Ok(new { Message = "Room deleted successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException )
        {
            return Forbid();
        }
    }

    [HttpGet("amenities")]
    public async Task<IActionResult> GetAllAmenities()
    {
        var amenities = await _roomService.GetAllAmenitiesAsync();
        return Ok(amenities);
    }
}
