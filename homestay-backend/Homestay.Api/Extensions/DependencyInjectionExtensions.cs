using Homestay.Api.Application.Interfaces;
using Homestay.Api.Application.Services;
using Homestay.Api.Infrastructure.Repositories;

namespace Homestay.Api.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<HomestayRepository>();
        services.AddScoped<IHomestayService, HomestayService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
