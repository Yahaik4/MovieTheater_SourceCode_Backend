using src.Infrastructure.EF.Models;

namespace src.Infrastructure.Repositories.Interfaces
{
    public interface IMoviePersonRepository
    {
        Task<IEnumerable<MoviePerson>> CreateMoviePerson(Guid movieId, List<Guid> personIds);
        Task<MoviePerson> UpdateMoviePerson(MoviePerson moviePerson);
        Task<bool> DeleteMoviePerson(Guid id);
    }
}
