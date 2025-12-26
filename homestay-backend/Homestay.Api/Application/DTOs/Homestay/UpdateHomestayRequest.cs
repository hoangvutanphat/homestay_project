namespace Homestay.Api.Application.DTOs;

public class UpdateHomestayRequest
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? Description { get; set; }
}
