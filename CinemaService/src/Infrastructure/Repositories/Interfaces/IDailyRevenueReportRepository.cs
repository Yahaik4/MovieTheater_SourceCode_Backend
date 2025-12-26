using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface IDailyRevenueReportRepository
    {
        Task<IEnumerable<DailyRevenueReport>> CreateDailyRevenueReports(DateOnly date);
        Task<IEnumerable<MovieRevenueReport>> CreateMovieRevenueReports(DateOnly date);
        Task<IEnumerable<FoodDrinkRevenueReport>> CreateFoodDrinkRevenueReports(DateOnly date);
    }
}
