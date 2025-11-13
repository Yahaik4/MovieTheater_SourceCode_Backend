using CinemaService.Data;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaService.Infrastructure.Repositories
{
    public class ShowtimeSeatRepository : IShowtimeSeatRepository
    {
        private readonly CinemaDbContext _context;

        public ShowtimeSeatRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShowtimeSeat>> GetShowtimeSeatsByShowtimeId(Guid showtimeId)
        {
            var showtimeSeats = await _context.ShowtimeSeats.Include(sts => sts.Seat)
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

        public async Task<IEnumerable<ShowtimeSeat>> CreateShowtimeSeats(Guid showtimeId, List<Guid> seatIds)
        {
            var showtimeSeats = seatIds.Select(seatId => new ShowtimeSeat
            {
                Id = Guid.NewGuid(),
                ShowTimeId = showtimeId,
                SeatId = seatId,
                Status = "Available"
            }).ToList();

            await _context.ShowtimeSeats.AddRangeAsync(showtimeSeats);
            await _context.SaveChangesAsync();

            return showtimeSeats;
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
    }
}
