using Homestay.Api.Application.DTOs.Auth;
using Homestay.Api.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        // Set httpOnly cookie
        Response.Cookies.Append("jwt_token", response.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            MaxAge = TimeSpan.FromDays(7)
        });

        // Return only user data, not token
        return Ok(new { user = response.User });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {   
        var response = await _authService.LoginAsync(request);
        if (response == null)
            return Unauthorized(new { Message = "Invalid email or password." });
        
        // Set httpOnly cookie
        Response.Cookies.Append("jwt_token", response.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            MaxAge = TimeSpan.FromDays(7)
        });
        
        // Return only user data, not token
        return Ok(new { user = response.User });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // Delete the jwt_token cookie
        Response.Cookies.Delete("jwt_token", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });
        
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        // Get user ID from JWT claims
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { Message = "Invalid token" });

        // Get user from database
        var user = await _authService.GetUserByIdAsync(userId);
        
        if (user == null)
            return NotFound(new { Message = "User not found" });

        return Ok(user);
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        // Get user ID from JWT claims
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { Message = "Invalid token" });

        // Validate request
        if (string.IsNullOrEmpty(request.CurrentPassword) || string.IsNullOrEmpty(request.NewPassword))
            return BadRequest(new { Message = "Current password and new password are required" });

        if (request.NewPassword.Length < 6)
            return BadRequest(new { Message = "New password must be at least 6 characters long" });

        if (request.CurrentPassword == request.NewPassword)
            return BadRequest(new { Message = "New password must be different from current password" });

        // Change password
        var result = await _authService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);
        
        if (!result)
            return BadRequest(new { Message = "Current password is incorrect" });

        return Ok(new { Message = "Password changed successfully" });
    }
}

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
