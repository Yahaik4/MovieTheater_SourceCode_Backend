using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface IPromotionRepository
    { 
        Task<IEnumerable<Promotion>> GetPromotions(Guid? id, string? code, DateTime? startDate, DateTime? endDate, string? discountType, bool? isActive);
        Task<Promotion?> GetPromotionById (Guid id);
        Task<Promotion?> GetPromotionByCode (string code);
        Task<Promotion> CreatePromotion(Promotion promotion);
        Task<Promotion> UpdatePromotion(Promotion promotion);
    }
}
