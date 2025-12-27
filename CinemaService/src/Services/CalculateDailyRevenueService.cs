using CinemaService.Infrastructure.Repositories.Interfaces;
using Quartz;

namespace CinemaService.Services
{
    public class CalculateDailyRevenueService : IJob
    {
        private readonly IDailyRevenueReportRepository _dailyRevenueReportRepository;

        public CalculateDailyRevenueService(IDailyRevenueReportRepository dailyRevenueReportRepository)
        {
            _dailyRevenueReportRepository = dailyRevenueReportRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var vnNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);

            var reportDate = DateOnly.FromDateTime(vnNow.Date);

            await _dailyRevenueReportRepository.CreateDailyRevenueReports(reportDate);
            await _dailyRevenueReportRepository.CreateMovieRevenueReports(reportDate);
            await _dailyRevenueReportRepository.CreateFoodDrinkRevenueReports(reportDate);
        }
    }
}