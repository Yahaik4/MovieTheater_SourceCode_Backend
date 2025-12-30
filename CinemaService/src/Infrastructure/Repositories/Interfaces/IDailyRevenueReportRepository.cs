using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface IDailyRevenueReportRepository
    {
        Task<IEnumerable<DailyRevenueReport>> CreateDailyRevenueReports(DateOnly date);
        Task<IEnumerable<MovieRevenueReport>> CreateMovieRevenueReports(DateOnly date);
        Task<IEnumerable<FoodDrinkRevenueReport>> CreateFoodDrinkRevenueReports(DateOnly date);
        Task<IEnumerable<DailyRevenueReport>> GetDailyRevenueReports(DateOnly from, DateOnly to, Guid? cinemaId);
        Task<IEnumerable<MovieRevenueReport>> GetMovieRevenueReports(DateOnly from, DateOnly to, Guid? cinemaId);
        Task<IEnumerable<FoodDrinkRevenueReport>> GetFoodDrinkRevenueReports(DateOnly from, DateOnly to, Guid? cinemaId);

    }
}
