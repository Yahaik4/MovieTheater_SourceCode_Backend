using src.DataTransferObject.Parameter;
using src.Infrastructure.EF.Models;

namespace src.Infrastructure.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetMovies(GetMoviesParam param);
        Task<Movie> CreateMove(Movie movie);
        Task<Movie> UpdateMovie(Movie movie);
        Task<bool> DeleteMovie(Guid id);
    }
}
