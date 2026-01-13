namespace Homestay.Api.Application.DTOs.Auth;

public class RegisterResponse
{
    public string Token { get; set; } = null!;
    public string Message { get; set; } = null!;
    public UserInfo User { get; set; } = null!;
}
