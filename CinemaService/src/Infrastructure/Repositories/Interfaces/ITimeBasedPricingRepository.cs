using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface ITimeBasedPricingRepository
    {
        Task<TimeBasedPricing?> GetTimeBasedPricingById(Guid id);
        Task<TimeBasedPricing?> GetTimeBasedPricingByTime(TimeOnly startTime);
    }
}
