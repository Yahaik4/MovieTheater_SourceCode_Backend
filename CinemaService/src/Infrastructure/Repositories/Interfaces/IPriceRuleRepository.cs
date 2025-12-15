using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface IPriceRuleRepository
    {
        Task<IEnumerable<PriceRule>> GetPriceRules();
        Task<PriceRule?> GetPriceRuleById(Guid id);
        Task<PriceRule?> GetApplicablePriceRuleAsync(Guid customerTypeId, int dayOfWeek, TimeOnly startTime);
    }
}
