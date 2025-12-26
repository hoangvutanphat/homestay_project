using Homestay.Api.Application.Interfaces;
using Homestay.Api.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api/homestays")]
public class HomestayController : ControllerBase
{
    private readonly IHomestayService _homestayService;

    public HomestayController(IHomestayService homestayService)
    {
        _homestayService = homestayService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    
      =>  Ok(await _homestayService.GetAllHomestaysAsync());
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
        { 
            var result = await _homestayService.GetHomestayByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }
    
    [HttpGet("admin/all-including-deleted")]
    [Authorize]
    public async Task<IActionResult> GetAllIncludingDeleted()
        => Ok(await _homestayService.GetAllHomestaysIncludingDeletedAsync());
    [HttpGet("admin/deleted")]
    [Authorize]
    public async Task<IActionResult> GetDeletedHomestays()
        => Ok(await _homestayService.GetDeletedHomestaysAsync());
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHomestayRequest request)
    {
        var newHomestay = await _homestayService.CreateHomestayAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = newHomestay.Id }, newHomestay);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHomestayRequest request)
    {
        var success = await _homestayService.UpdateHomestayAsync(id, request);
        return success ? NoContent() : NotFound();
    }
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid? deletedBy = userIdClaim != null ? Guid.Parse(userIdClaim) : null;
        
        var result = await _homestayService.SoftDeleteHomestayAsync(id, deletedBy);
        if(!result)
            return NotFound(
                new { Message = "Homestay not found or already deleted." });
        return Ok(
            new { Message = "Homestay soft-deleted successfully." 
            , Id = id, DeletedBy = deletedBy, DeletedAt = DateTime.UtcNow});
    }
    [HttpPost("{id}/restore")]
    //Authorize
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _homestayService.RestoreHomestayAsync(id);
        if (!result)
        return NotFound(
            new { Message = "Homestay not found or is not deleted." });
        return Ok(
            new { Message = "Homestay restored successfully." }
            );
    }
    public async Task<IActionResult> PermanentlyDelete(Guid id, [FromQuery] bool confirm = false)
    {
        if (!confirm)
        return BadRequest(new 
        { 
            Message = "Please confirm permanent deletion by setting 'confirm' query parameter to true." 
        });
    
    var result = await _homestayService.PermanentlyDeleteHomestayAsync(id);
    
    if (!result)
        return NotFound(new 
        { 
            Message = "Homestay not found." 
        });
    
    return Ok(new 
    { 
        Message = "Homestay permanently deleted successfully.",
        Id = id
    });
    }
}