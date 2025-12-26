using System.ComponentModel.DataAnnotations;

namespace Homestay.Api.Application.DTOs.Auth;

public class RegisterRequest
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string FirstName { get; set; } = null!;
    
    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string LastName { get; set; } = null!;
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", 
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number")]
    public string Password { get; set; } = null!;
    
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? Phone { get; set; }
    
    [Required(ErrorMessage = "Role is required")]
    [RegularExpression("^(guest|host|admin)$", ErrorMessage = "Role must be guest, host, or admin")]
    public string Role { get; set; } = "guest";
}
