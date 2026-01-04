using Homestay.Api.Domain.Constants;
using Homestay.Api.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Homestay.Api.Application.Services;

public class BookingExpirationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BookingExpirationService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

    public BookingExpirationService(IServiceProvider serviceProvider, ILogger<BookingExpirationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Booking Expiration Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessExpiredBookingsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing expired bookings");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("Booking Expiration Service stopped");
    }

    private async Task ProcessExpiredBookingsAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var bookingRepository = scope.ServiceProvider.GetRequiredService<BookingRepository>();
        var availabilityRepository = scope.ServiceProvider.GetRequiredService<RoomAvailabilityRepository>();

        await availabilityRepository.ReleaseExpiredReservationsAsync();

        var expiredBookings = await bookingRepository.GetExpiredPendingPaymentBookingsAsync();

        foreach (var booking in expiredBookings)
        {
            _logger.LogInformation("Expiring booking {BookingId}", booking.Id);
            
            await bookingRepository.UpdateStatusAsync(booking.Id, BookingStatus.Cancelled);
            
            await availabilityRepository.ReleaseRoomAsync(
                booking.RoomId, booking.CheckIn, booking.CheckOut, booking.Id);
        }

        if (expiredBookings.Any())
        {
            _logger.LogInformation("Expired {Count} bookings", expiredBookings.Count());
        }
    }
}
