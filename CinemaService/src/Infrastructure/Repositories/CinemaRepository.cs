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
            await _context.AddAsync(cinema);
            await _context.SaveChangesAsync();
            return cinema;
        }

        public async Task<IEnumerable<Cinema>> GetAllCinema()
        {
            return await _context.Cinemas.ToListAsync();
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
