using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface ISpecialDayPricingRepository
    {
        Task<SpecialDayPricing?> GetSpecialDayPricingById(Guid id);
        Task<SpecialDayPricing?> GetSpecialDayPricingByDay(DateOnly date);
    }
}
