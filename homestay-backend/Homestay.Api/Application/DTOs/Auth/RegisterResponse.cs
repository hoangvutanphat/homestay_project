namespace Homestay.Api.Application.DTOs.Auth;

public class RegisterResponse
{
    public string Message { get; set; } = null!;
    public UserInfo User { get; set; } = null!;
}
