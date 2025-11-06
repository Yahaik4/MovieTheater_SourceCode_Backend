using src.Data;
using src.DataTransferObject.Parameter;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.Infrastructure.Repositories
{
    public class MovieGenreRepository : IMovieGenreRepository
    {
        private readonly MovieDbContext _context;
        public MovieGenreRepository(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovieGenre>> CreateMovieGenre(Guid movieId, List<MovieGenreParam> genres)
        {
            var movieGenres = genres.Select(p => new MovieGenre
            {
                MovieId = movieId,
                GenreId = p.GenreId,
            }).ToList();

            _context.MovieGenres.AddRange(movieGenres);
            await _context.SaveChangesAsync();

            return movieGenres;
        }

        public Task<bool> DeleteMovieGenre(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<MovieGenre> UpdateMovieGenre(MovieGenre movieGenre)
        {
            throw new NotImplementedException();
        }
    }
}
