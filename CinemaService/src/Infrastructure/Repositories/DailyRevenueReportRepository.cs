using CinemaService.Data;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaService.Infrastructure.Repositories
{
    public class DailyRevenueReportRepository : IDailyRevenueReportRepository
    {
        private readonly CinemaDbContext _context;

        public DailyRevenueReportRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DailyRevenueReport>> CreateDailyRevenueReports(DateOnly reportDate)
        {
            var vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            var startVn = reportDate.ToDateTime(TimeOnly.MinValue);
            var startUtc = TimeZoneInfo.ConvertTimeToUtc(startVn, vnZone);
            var endUtc = startUtc.AddDays(1);

            var existedCinemaIds = await _context.DailyRevenueReports
                .Where(x => x.Date == startVn)
                .Select(x => x.CinemaId)
                .ToListAsync();

            var dailyRevenueData = await
                (from b in _context.Bookings
                 join s in _context.Showtimes on b.ShowtimeId equals s.Id
                 join r in _context.Rooms on s.RoomId equals r.Id
                 join c in _context.Cinemas on r.CinemaId equals c.Id
                 where b.CreatedAt >= startUtc
                    && b.CreatedAt < endUtc
                    && b.Status == "paid"
                 group b by c.Id into g
                 select new
                 {
                     CinemaId = g.Key,
                     TicketSold = g.Count(),
                     Sales = g.Sum(x => x.TotalPrice)
                 }).ToListAsync();

            var reports = dailyRevenueData
                .Where(x => !existedCinemaIds.Contains(x.CinemaId))
                .Select(x => new DailyRevenueReport
                {
                    Id = Guid.NewGuid(),
                    Date = startVn,
                    CinemaId = x.CinemaId,
                    TicketSold = x.TicketSold,
                    Sales = x.Sales,
                    ProjectedProfit = x.Sales * 0.3m
                }).ToList();

            if (reports.Any())
            {
                _context.DailyRevenueReports.AddRange(reports);
                await _context.SaveChangesAsync();
            }

            return reports;
        }

        public Task<IEnumerable<FoodDrinkRevenueReport>> CreateFoodDrinkRevenueReports(DateOnly date)
        {
            throw new NotImplementedException();
        }


        //public async Task<IEnumerable<FoodDrinkRevenueReport>> CreateFoodDrinkRevenueReports(DateOnly reportDate)
        //{
        //    var vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        //    var startVn = reportDate.ToDateTime(TimeOnly.MinValue);
        //    var startUtc = TimeZoneInfo.ConvertTimeToUtc(startVn, vnZone);
        //    var endUtc = startUtc.AddDays(1);

        //    var existedFoodDrinkIds = await _context.FoodDrinkRevenueReports
        //        .Where(x => x.Date == startVn)
        //        .Select(x => x.FoodDrinkId)
        //        .ToListAsync();

        //    var revenueData = await
        //        (from fd in _context.FoodDrinkOrderDetails
        //         join o in _context.FoodDrinkOrders on fd.OrderId equals o.Id
        //         join b in _context.Bookings on o.BookingId equals b.Id
        //         where b.CreatedAt >= startUtc
        //            && b.CreatedAt < endUtc
        //            && b.Status == "paid"
        //         group fd by fd.FoodDrinkId into g
        //         select new
        //         {
        //             FoodDrinkId = g.Key,
        //             QuantitySold = g.Sum(x => x.Quantity),
        //             Sales = g.Sum(x => x.Price * x.Quantity)
        //         }).ToListAsync();

        //    var reports = revenueData
        //        .Where(x => !existedFoodDrinkIds.Contains(x.FoodDrinkId))
        //        .Select(x => new FoodDrinkRevenueReport
        //        {
        //            Id = Guid.NewGuid(),
        //            Date = startVn,
        //            FoodDrinkId = x.FoodDrinkId,
        //            QuantitySold = x.QuantitySold,
        //            Sales = x.Sales,
        //            ProjectedProfit = x.Sales * 0.25m
        //        }).ToList();

        //    if (reports.Any())
        //    {
        //        _context.FoodDrinkRevenueReports.AddRange(reports);
        //        await _context.SaveChangesAsync();
        //    }

        //    return reports;
        //}


        public Task<IEnumerable<MovieRevenueReport>> CreateMovieRevenueReports(DateOnly date)
        {
            throw new NotImplementedException();
        }
    }
}
