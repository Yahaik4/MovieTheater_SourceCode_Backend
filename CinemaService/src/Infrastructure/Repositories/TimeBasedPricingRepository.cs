//using CinemaService.Data;
//using CinemaService.Infrastructure.EF.Models;
//using CinemaService.Infrastructure.Repositories.Interfaces;
//using Microsoft.EntityFrameworkCore;

//namespace CinemaService.Infrastructure.Repositories
//{
//    public class TimeBasedPricingRepository : ITimeBasedPricingRepository
//    {
//        private readonly CinemaDbContext _context;

//        public TimeBasedPricingRepository(CinemaDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<TimeBasedPricing?> GetTimeBasedPricingById(Guid id)
//        {
//            return await _context.TimeBasedPricings.FirstOrDefaultAsync(t => t.Id == id);
//        }

//        public async Task<TimeBasedPricing?> GetTimeBasedPricingByTime(TimeOnly startTime)
//        {
//            return await _context.TimeBasedPricings.FirstOrDefaultAsync(t => t.StartTime <= startTime && startTime < t.EndTime);
//        }
//    }
//}
