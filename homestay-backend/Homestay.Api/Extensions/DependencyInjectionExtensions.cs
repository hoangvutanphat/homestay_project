using Homestay.Api.Application.Interfaces;
using Homestay.Api.Application.Services;
using Homestay.Api.Infrastructure.Repositories;

namespace Homestay.Api.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<HomestayRepository>();
        services.AddScoped<BookingRepository>();
        services.AddScoped<RoomRepository>();
        services.AddScoped<IHomestayService, HomestayService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IRoomService, RoomService>();

        return services;
    }
}
