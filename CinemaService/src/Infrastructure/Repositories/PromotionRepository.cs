using CinemaService.Data;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace CinemaService.Infrastructure.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly CinemaDbContext _context;

        public PromotionRepository(CinemaDbContext context)
        {
            _context = context;
        }
        public async Task<Promotion?> GetPromotionByCode(string code)
        {
            return await _context.Promotions.FirstOrDefaultAsync(p => p.Code == code);
        }

        public async Task<Promotion?> GetPromotionById(Guid id)
        {
            return await _context.Promotions.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Promotion>> GetPromotions(Guid? id, string? code, DateTime? startDate, DateTime? endDate, string? discountType, bool? isActive)
        {
            var query = _context.Promotions.AsQueryable();

            if (id.HasValue)
                query = query.Where(p => p.Id == id.Value);

            if (!string.IsNullOrWhiteSpace(code))
                query = query.Where(p => p.Code.Contains(code));

            if (startDate.HasValue)
                query = query.Where(p => p.StartDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p => p.EndDate <= endDate.Value);

            if (!string.IsNullOrWhiteSpace(discountType))
                query = query.Where(p => p.DiscountType == discountType);

            if (isActive.HasValue)
                query = query.Where(p => p.IsActive == isActive.Value);

            return await query.ToListAsync();
        }

        public async Task<Promotion> CreatePromotion(Promotion promotion)
        {
            await _context.Promotions.AddAsync(promotion);
            await _context.SaveChangesAsync();
            return promotion;
        }

        public async Task<Promotion> UpdatePromotion(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            await _context.SaveChangesAsync();
            return promotion;
        }

        public async Task<Promotion?> SearchPromotionByCode(string code)
        {
            return await _context.Promotions.Where(p => p.Code == code && p.EndDate > DateTime.Now && p.IsActive && !p.IsDeleted).FirstOrDefaultAsync();
        }
    }
}
