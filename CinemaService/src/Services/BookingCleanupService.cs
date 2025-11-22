using CinemaService.Infrastructure.Repositories.Interfaces;

public class BookingCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public BookingCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var bookingRepo = scope.ServiceProvider.GetRequiredService<IBookingRepository>();

                await bookingRepo.UpdateExpiredBookingsStatusAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BookingCleanup] Error: {ex.Message}");
            }

            // update booking mỗi 1 phút
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
