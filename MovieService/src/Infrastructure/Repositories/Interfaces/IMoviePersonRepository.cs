using MovieService.DataTransferObject.Parameter;
using MovieService.Infrastructure.EF.Models;

namespace MovieService.Infrastructure.Repositories.Interfaces
{
    public interface IMoviePersonRepository
    {
        Task<IEnumerable<MoviePerson>> CreateMoviePerson(Guid movieId, List<MoviePersonParam> persons);
        Task<MoviePerson> UpdateMoviePerson(MoviePerson moviePerson);
        Task<bool> DeleteMoviePerson(Guid movieId, List<Guid> personIds);
    }
}
