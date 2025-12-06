using CinemaService.Data;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaService.Infrastructure.Repositories
{
    public class CinemaRepository : ICinemaRepository
    {
        private readonly CinemaDbContext _context;

        public CinemaRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<Cinema> CreateCinema(Cinema cinema)
        {
            await _context.Cinemas.AddAsync(cinema);
            await _context.SaveChangesAsync();
            return cinema;
        }

        public async Task<IEnumerable<Cinema>> GetAllCinema(Guid? id, string? name, string? city, string? status)
        {
            var query = _context.Cinemas.AsQueryable();

            if (id.HasValue)
                query = query.Where(x => x.Id == id);

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(x => x.City.ToLower().Contains(city.ToLower()));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(x => x.Status == status);

            query = query.Where(x => x.IsDeleted == false);

            return await query.ToListAsync();
        }

        public async Task<Cinema?> GetCinemaById(Guid cinemaId)
        {
           return await _context.Cinemas.FirstOrDefaultAsync(c => c.Id == cinemaId);
        }

        public async Task<Cinema> UpdateCinema(Cinema cinema)
        {
            _context.Cinemas.Update(cinema);
            await _context.SaveChangesAsync();
            return cinema;
        }

        public async Task<IEnumerable<Cinema>> GetCinemasWithShowtimes(Guid movieId, DateOnly date, string? country)
        {
            var startDate = DateTime.SpecifyKind(date.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
            var endDate = DateTime.SpecifyKind(date.ToDateTime(TimeOnly.MaxValue), DateTimeKind.Utc);

            return await _context.Cinemas
                .Include(c => c.Rooms.Where(r => !r.IsDeleted))
                    .ThenInclude(r => r.Showtimes
                        .Where(st => st.Status == "open"
                                     && st.MovieId == movieId
                                     && st.StartTime >= startDate
                                     && st.StartTime <= endDate))
                .Include(c => c.Rooms)
                    .ThenInclude(r => r.RoomType)
                .Where(c => (country == null || c.City == country) && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Showtime>> GetAllShowtimesAsync(Guid? cinemaId, Guid? movieId, DateOnly? date)
        {
            var query = _context.Showtimes
                .Include(st => st.Room)
                    .ThenInclude(r => r.Cinema)
                .Include(st => st.Room)
                    .ThenInclude(r => r.RoomType)
                .Where(st => !st.IsDeleted &&
                            !st.Room.IsDeleted &&
                            !st.Room.Cinema.IsDeleted)
                .AsQueryable();

            if (cinemaId.HasValue)
            {
                query = query.Where(st => st.Room.CinemaId == cinemaId.Value);
            }

            if (movieId.HasValue)
            {
                query = query.Where(st => st.MovieId == movieId.Value);
            }

            if (date.HasValue)
            {
                var start = date.Value.ToDateTime(TimeOnly.MinValue);
                var end = date.Value.ToDateTime(TimeOnly.MaxValue);
                query = query.Where(st => st.StartTime >= start && st.StartTime <= end);
            }

            // nếu bạn muốn chỉ lấy status "open" thì thêm:
            query = query.Where(st => st.Status == "open");

            return await query
                .OrderBy(st => st.StartTime)
                .ToListAsync();
        }
    }
}
