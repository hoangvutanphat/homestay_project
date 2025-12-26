using Homestay.Api.Application.DTOs.Auth;

namespace Homestay.Api.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    Task<RegisterResponse?> RegisterAsync(RegisterRequest request);
    Task<bool> UserExistsAsync(string email);
}
