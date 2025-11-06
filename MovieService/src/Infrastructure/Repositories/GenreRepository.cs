using Microsoft.EntityFrameworkCore;
using src.Data;
using src.DataTransferObject.Parameter;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly MovieDbContext _context;

        public GenreRepository(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> CreateGenre(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public Task<bool> DeleteGenre(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Genre?> GetGenreById(Guid id)
        {
            return await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);

        }

        public async Task<IEnumerable<Genre>> GetGenreByIds(List<Guid> ids)
        {
            return await _context.Genres.Where(g => ids.Contains(g.Id)).ToListAsync();
        }

        public async Task<Genre?> GetGenreByName(string name)
        {
            return await _context.Genres.FirstOrDefaultAsync(g => g.Name == name);
        }

        public async Task<IEnumerable<Genre>> GetGenres(GetGenresParam param)
        {
            var query = _context.Genres.AsQueryable();

            if (param.Id.HasValue)
                query = query.Where(g => g.Id == param.Id);

            if (!string.IsNullOrWhiteSpace(param.Name))
                query = query.Where(g => g.Name.ToLower().Contains(param.Name.ToLower()));

            query = query.Where(x => x.IsDeleted == false);

            return await query.ToListAsync();
        }

        public async Task<Genre> UpdateGenre(Genre genre)
        {
            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
            return genre;
        }
    }
}
