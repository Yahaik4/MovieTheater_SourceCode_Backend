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
    }
}
