using CinemaService.Data;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace CinemaService.Infrastructure.Repositories
{
    public class PriceRuleRepository : IPriceRuleRepository
    {
        private readonly CinemaDbContext _context;

        public PriceRuleRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<PriceRule?> GetApplicablePriceRuleAsync(Guid customerTypeId, int dayOfWeek, TimeOnly startTime)
        {
            return await _context.PriceRules
                .FirstOrDefaultAsync(r =>
                    r.CustomerTypeId == customerTypeId &&
                    r.DayOfWeek == dayOfWeek &&
                    r.StartTime <= startTime &&
                    r.EndTime > startTime);
        }

        public async Task<PriceRule?> GetPriceRuleById(Guid id)
        {
            return await _context.PriceRules.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<PriceRule>> GetPriceRules()
        {
            return await _context.PriceRules.ToListAsync();
        }
    }
}
