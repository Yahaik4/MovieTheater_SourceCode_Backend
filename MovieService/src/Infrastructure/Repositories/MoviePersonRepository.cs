using Microsoft.EntityFrameworkCore;
using src.Data;
using src.DataTransferObject.Parameter;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.Infrastructure.Repositories
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

        public Task<bool> DeleteMoviePerson(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<MoviePerson> UpdateMoviePerson(MoviePerson moviePerson)
        {
            throw new NotImplementedException();
        }
    }
}
