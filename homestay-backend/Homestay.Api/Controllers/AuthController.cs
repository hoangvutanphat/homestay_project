using Homestay.Api.Application.DTOs.Auth;
using Homestay.Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Homestay.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {   
        if (await _authService.UserExistsAsync(request.Email))
            return Conflict(new { Message = "User with this email already exists." });

        var response = await _authService.RegisterAsync(request);
        if (response == null)
            return BadRequest(new { Message = "Registration failed." });

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {   
        var response = await _authService.LoginAsync(request);
        if (response == null)
            return Unauthorized(new { Message = "Invalid email or password." });

        return Ok(response);
    }
}
