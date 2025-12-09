using Microsoft.EntityFrameworkCore;
using MovieService.Data;
using MovieService.DataTransferObject.Parameter;
using MovieService.Infrastructure.EF.Models;
using MovieService.Infrastructure.Repositories.Interfaces;

namespace MovieService.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDbContext _context;

        public MovieRepository(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> CreateMovie(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public Task<bool> DeleteMovie(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Movie?> GetMovieById(Guid id)
        {
            return await _context.Movies
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(m => m.MoviePersons)
                    .ThenInclude(mp => mp.Person)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<IEnumerable<Movie>> GetMoviesByIds(IEnumerable<Guid> movieIds)
        {
            return await _context.Movies.Include(m => m.MovieGenres)
                                            .ThenInclude(mg => mg.Genre)
                                        .Include(m => m.MoviePersons)
                                            .ThenInclude(mp => mp.Person)
                .Where(m => movieIds.Contains(m.Id) && !m.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetMovies(GetMoviesParam param)
        {
            var query = _context.Movies
                .Include(m => m.MovieGenres)
                    .ThenInclude(mp => mp.Genre)
                .Include(m => m.MoviePersons)
                    .ThenInclude(mg => mg.Person)
                .AsQueryable();

            if (param.Id.HasValue)
                query = query.Where(m => m.Id == param.Id);

            if (!string.IsNullOrWhiteSpace(param.Name))
                query = query.Where(m => m.Name.ToLower().Contains(param.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(param.Country))
                query = query.Where(m => m.Country.ToLower().Contains(param.Country.ToLower()));

            if (!string.IsNullOrWhiteSpace(param.Status))
                query = query.Where(m => m.Status.ToLower().Contains(param.Status.ToLower()));

            query = query.Where(x => x.IsDeleted == false);

            return await query.ToListAsync();
        }

        public async Task<Movie> UpdateMovie(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
            return movie;
        }
    }
}
