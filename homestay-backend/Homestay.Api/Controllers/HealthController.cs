using Microsoft.AspNetCore.Mvc;
using Homestay.Api.Infrastructure.Persistence;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    private readonly HomestayDbContext _context;

    public HealthController(HomestayDbContext context)
    {
        _context = context;
    }

    [HttpGet("db")]
    public IActionResult CheckDatabase()
    {
        return Ok(new
        {
            Homestays = _context.Homestays.Count(),
            Users = _context.Users.Count(),
        });
    }
}