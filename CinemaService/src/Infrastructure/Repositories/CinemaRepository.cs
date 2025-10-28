using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.Infrastructure.Repositories
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

    }
}
