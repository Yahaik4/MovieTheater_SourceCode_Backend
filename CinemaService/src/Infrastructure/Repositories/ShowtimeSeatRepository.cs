using CinemaService.Data;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CinemaService.Infrastructure.Repositories
{
    public class ShowtimeSeatRepository : IShowtimeSeatRepository
    {
        private readonly CinemaDbContext _context;

        public ShowtimeSeatRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<List<ShowtimeSeat>> GetSeatsForBookingAsync(List<Guid> seatIds, Guid showtimeId)
        {
            var seatIdsArray = seatIds.ToArray();

            // Lock the ShowtimeSeats rows
            var showtimeSeats = await _context.ShowtimeSeats
                .FromSqlInterpolated($@"
                    SELECT * FROM ""ShowtimeSeats""
                    WHERE ""ShowTimeId"" = {showtimeId}
                      AND ""Id"" = ANY({seatIdsArray})
                      AND ""Status"" = 'available'
                    FOR UPDATE
                ")
                .ToListAsync();

            // Load related Seat data (within the same transaction)
            if (showtimeSeats.Any())
            {
                var seatIdsToLoad = showtimeSeats.Select(sts => sts.SeatId).Distinct().ToArray();

                var seats = await _context.Seats
                    .Where(s => seatIdsToLoad.Contains(s.Id))
                    .Include(s => s.SeatType)
                    .ToListAsync();

                // Map seats to showtimeSeats
                var seatDict = seats.ToDictionary(s => s.Id);
                foreach (var sts in showtimeSeats)
                {
                    if (seatDict.TryGetValue(sts.SeatId, out var seat))
                    {
                        sts.Seat = seat;
                    }
                }
            }

            return showtimeSeats;
        }

        public async Task<IEnumerable<ShowtimeSeat>> GetShowtimeSeatsByShowtimeId(Guid showtimeId)
        {
            var showtimeSeats = await _context.ShowtimeSeats.Include(sts => sts.Seat)
                                                                .ThenInclude(s => s.SeatType)
                                                            .Include(sts => sts.Showtime)
                                                                .ThenInclude(st => st.Room)
                                                            .Where(sts => sts.ShowTimeId == showtimeId)
                                                            .ToListAsync();

            return showtimeSeats
                .OrderBy(sts => sts.Seat.Label[0])
                .ThenBy(sts =>
                {
                    var numberPart = new string(sts.Seat.Label.Skip(1).ToArray());
                    return int.TryParse(numberPart, out var num) ? num : 0;
                })
                .ToList();
        }

        public async Task<IEnumerable<ShowtimeSeat>> CreateShowtimeSeats(Guid showtimeId, List<Guid> seatIds, decimal basePriceRoom)
        {
            var seats = await _context.Seats.Include(s => s.SeatType).Where(s => seatIds.Contains(s.Id)).ToListAsync();

            var showtimeSeats = seats.Select(seat => new ShowtimeSeat
            {
                Id = Guid.NewGuid(),
                ShowTimeId = showtimeId,
                SeatId = seat.Id,
                Status = "available",
                Price = basePriceRoom + seat.SeatType.ExtraPrice,
            }).ToList();

            await _context.ShowtimeSeats.AddRangeAsync(showtimeSeats);
            await _context.SaveChangesAsync();

            return showtimeSeats;
        }

        public async Task UpdateSeatsAsync(List<ShowtimeSeat> seats)
        {
            _context.ShowtimeSeats.UpdateRange(seats);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteShowtimeSeatsByShowtimeId(Guid showtimeId)
        {
            var showtimeseats = await _context.ShowtimeSeats.Where(sts => sts.ShowTimeId == showtimeId).ToListAsync();

            if (showtimeseats.Any())
            {
                _context.ShowtimeSeats.RemoveRange(showtimeseats);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task MarkSeatsAsAvailable(Guid showtimeId, List<Guid> seatIds)
        {
            var showtimeseats = await _context.ShowtimeSeats.Where(x => x.ShowTimeId == showtimeId && seatIds.Contains(x.SeatId)).ToListAsync();
                
            foreach(var showtimeseat in showtimeseats)
            {
                showtimeseat.Status = "available";
            }

            await _context.SaveChangesAsync();
        }

    }
}
