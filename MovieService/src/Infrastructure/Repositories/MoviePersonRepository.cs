using Microsoft.EntityFrameworkCore;
using MovieService.Data;
using MovieService.DataTransferObject.Parameter;
using MovieService.Infrastructure.EF.Models;
using MovieService.Infrastructure.Repositories.Interfaces;

namespace MovieService.Infrastructure.Repositories
{
    public class MoviePersonRepository : IMoviePersonRepository
    {
        private readonly MovieDbContext _context;
        public MoviePersonRepository(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MoviePerson>> CreateMoviePerson(Guid movieId, List<MoviePersonParam> persons)
        {
            var moviePersons = persons.Select(p => new MoviePerson
            {
                MovieId = movieId,
                PersonId = p.PersonId,
                Role = p.Role
            }).ToList();

            _context.MoviePersons.AddRange(moviePersons);
            await _context.SaveChangesAsync();

            return moviePersons;
        }

        public async Task<bool> DeleteMoviePerson(Guid movieId, List<Guid> personIds)
        {
            var moviePersons = await _context.MoviePersons.Where(mp => mp.MovieId == movieId && personIds.Contains(mp.PersonId)).ToListAsync();

            if (!moviePersons.Any())
                return false;

            _context.MoviePersons.RemoveRange(moviePersons);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<MoviePerson> UpdateMoviePerson(MoviePerson moviePerson)
        {
            throw new NotImplementedException();
        }
    }
}
