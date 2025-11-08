using MovieService.DataTransferObject.Parameter;
using MovieService.Infrastructure.EF.Models;

namespace MovieService.Infrastructure.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetMovies(GetMoviesParam param);
        Task<Movie?> GetMovieById(Guid id);
        Task<Movie> CreateMovie(Movie movie);
        Task<Movie> UpdateMovie(Movie movie);
        Task<bool> DeleteMovie(Guid id);
    }
}
