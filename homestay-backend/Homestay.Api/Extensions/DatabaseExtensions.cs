using Homestay.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Homestay.Api.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<HomestayDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .LogTo(Console.WriteLine, LogLevel.Information));

        return services;
    }
}
