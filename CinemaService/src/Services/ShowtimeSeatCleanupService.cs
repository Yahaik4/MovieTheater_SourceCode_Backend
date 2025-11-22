using CinemaService.Infrastructure.Repositories.Interfaces;

public class ShowtimeSeatCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ShowtimeSeatCleanupService(IServiceProvider serviceProvider)
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
                var showtimeSeatRepo = scope.ServiceProvider.GetRequiredService<IShowtimeRepository>();

                await showtimeSeatRepo.CompleteEndedShowtimesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ShowtimeSeatCleanup] Error: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
