using CinemaService.Data;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Grpc.Core;
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
            var startUtc = reportDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            var endUtc = startUtc.AddDays(1);

            var existedCinemaIds = await _context.DailyRevenueReports
                .Where(x => x.Date == reportDate)
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
                    Date = reportDate,
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

        public async Task<IEnumerable<FoodDrinkRevenueReport>> CreateFoodDrinkRevenueReports(DateOnly reportDate)
        {
            var startUtc = reportDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            var endUtc = startUtc.AddDays(1);

            var existed = await _context.FoodDrinkRevenueReports
                .Where(x => x.Date == reportDate)
                .Select(x => new { x.CinemaId, x.FoodDrinkId })
                .ToListAsync();

            var data = await
                (from bi in _context.BookingItems
                 join b in _context.Bookings on bi.BookingId equals b.Id
                 join s in _context.Showtimes on b.ShowtimeId equals s.Id
                 join r in _context.Rooms on s.RoomId equals r.Id
                 join c in _context.Cinemas on r.CinemaId equals c.Id
                 join f in _context.FoodDrinks on bi.ItemId equals f.Id
                 where b.CreatedAt >= startUtc
                    && b.CreatedAt < endUtc
                    && b.Status == "paid"
                 group bi by new { CinemaId = c.Id, ItemId = f.Id } into g
                 select new
                 {
                     CinemaId = g.Key.CinemaId,
                     FoodDrinkId = g.Key.ItemId,
                     QuantitySold = g.Sum(x => x.Quantity),
                     Sales = g.Sum(x => x.TotalPrice)
                 }).ToListAsync();

            var reports = data.Where(x => !existed.Any(e =>
                                    e.CinemaId == x.CinemaId &&
                                    e.FoodDrinkId == x.FoodDrinkId))
                                .Select(x => new FoodDrinkRevenueReport
                                {
                                    Id = Guid.NewGuid(),
                                    Date = reportDate,
                                    CinemaId = x.CinemaId,
                                    FoodDrinkId = x.FoodDrinkId,
                                    Quantity = x.QuantitySold,
                                    Sales = x.Sales,
                                    ProjectedProfit = x.Sales * 0.3m
                                }).ToList();

            if (reports.Any())
            {
                _context.FoodDrinkRevenueReports.AddRange(reports);
                await _context.SaveChangesAsync();
            }

            return reports;
        }

        public async Task<IEnumerable<MovieRevenueReport>> CreateMovieRevenueReports(DateOnly reportDate)
        {
            var startUtc = reportDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            var endUtc = startUtc.AddDays(1);

            var data = await 
                (from b in _context.Bookings
                join s in _context.Showtimes on b.ShowtimeId equals s.Id
                join r in _context.Rooms on s.RoomId equals r.Id
                where b.CreatedAt >= startUtc
                    && b.CreatedAt < endUtc
                    && b.Status == "paid"
                group b by new { r.CinemaId, s.MovieId } into g
                select new
                {
                    CinemaId = g.Key.CinemaId,
                    MovieId = g.Key.MovieId,
                    TicketSold = g.Count(),
                    Sales = g.Sum(x => x.TotalPrice)
                }).ToListAsync();


            var existed = await _context.MovieRevenueReports.Where(x => x.Date == reportDate)
                                                            .Select(x => new { x.CinemaId, x.MovieId })
                                                            .ToListAsync();

            var reports = data.Where(x => !existed.Any(e => e.CinemaId == x.CinemaId && e.MovieId == x.MovieId))
            .Select(x => new MovieRevenueReport
            {
                Id = Guid.NewGuid(),
                Date = reportDate,
                CinemaId = x.CinemaId,
                MovieId = x.MovieId,
                TicketSold = x.TicketSold,
                Sales = x.Sales,
                ProjectedProfit = x.Sales * 0.3m
            }).ToList();

            if (reports.Any())
            {
                _context.MovieRevenueReports.AddRange(reports);
                await _context.SaveChangesAsync();
            }

            return reports;
        }

        public async Task<IEnumerable<DailyRevenueReport>> GetDailyRevenueReports(DateOnly from, DateOnly to, Guid? cinemaId)
        {
            var query = _context.DailyRevenueReports.Where(x => x.Date >= from && x.Date <= to && x.IsDeleted == false);

            if (cinemaId.HasValue)
                query = query.Where(x => x.CinemaId == cinemaId.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<FoodDrinkRevenueReport>> GetFoodDrinkRevenueReports(DateOnly from, DateOnly to, Guid? cinemaId)
        {
            var query = _context.FoodDrinkRevenueReports.Where(x => x.Date >= from && x.Date <= to && x.IsDeleted == false);

            if (cinemaId.HasValue)
                query = query.Where(x => x.CinemaId == cinemaId.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<MovieRevenueReport>> GetMovieRevenueReports(DateOnly from, DateOnly to, Guid? cinemaId)
        {
            var query = _context.MovieRevenueReports.Where(x => x.Date >= from && x.Date <= to && x.IsDeleted == false);

            if (cinemaId.HasValue)
                query = query.Where(x => x.CinemaId == cinemaId.Value);

            return await query.ToListAsync();
        }
    }
}
