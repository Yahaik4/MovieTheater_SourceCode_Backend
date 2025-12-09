using CinemaService.Data;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaService.Infrastructure.Repositories
{
    public class ShowtimeRepository : IShowtimeRepository
    {
        private readonly CinemaDbContext _context;

        public ShowtimeRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<Showtime> CreateShowtime(Showtime showtime)
        {
            await _context.Showtimes.AddAsync(showtime);
            await _context.SaveChangesAsync();
            return showtime;
        }

        public async Task<Showtime?> GetShowtimeById(Guid id)
        {
            return await _context.Showtimes.Include(s => s.Room).FirstOrDefaultAsync(st => st.Id == id);
        }

        public async Task<IEnumerable<Showtime>> GetShowtimesByRoomId(Guid roomId)
        {
            return await _context.Showtimes.Where(st => st.RoomId == roomId).ToListAsync();
        }

        public async Task<Showtime> UpdateShowtime(Showtime showtime)
        {
            _context.Showtimes.Update(showtime);
            await _context.SaveChangesAsync();
            return showtime;
        }

        public async Task<IEnumerable<Showtime>> GetShowtimesByCinemaAndDate(Guid cinemaId, DateTime startDate,DateTime endDate)
        {
            return await _context.Showtimes.Include(st => st.Room)
                                            .ThenInclude(r => r.RoomType)
                                            .Where(st => st.Room.CinemaId == cinemaId
                                                      && st.Status == "open"
                                                      && st.StartTime >= startDate
                                                      && st.StartTime <= endDate
                                                      && !st.Room.IsDeleted)
                                            .ToListAsync();
        }


        public async Task CompleteEndedShowtimesAsync()
        {
            const int batchSize = 1000;

            var endedShowtimes = await _context.Showtimes
                .Where(s => s.EndTime < DateTime.UtcNow && s.Status != "Completed")
                .ToListAsync();

            if (endedShowtimes.Count == 0)
                return;

            foreach (var show in endedShowtimes)
            {
                show.Status = "completed";
            }

            await _context.SaveChangesAsync();

            bool hasMore = true;

            while (hasMore)
            {
                var seatsToDelete = await _context.ShowtimeSeats
                    .Where(ss => ss.Showtime.EndTime < DateTime.UtcNow)
                    .Take(batchSize)
                    .ToListAsync();

                if (seatsToDelete.Count == 0)
                {
                    hasMore = false;
                    break;
                }

                _context.ShowtimeSeats.RemoveRange(seatsToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
