using src.DataTransferObject.Parameter;
using src.Infrastructure.EF.Models;

namespace src.Infrastructure.Repositories.Interfaces
{
    public interface IMovieGenreRepository
    {
        //Task<IEnumerable<Genre>> GetGenres(GetGenresParam param);
        Task<IEnumerable<MovieGenre>> CreateMovieGenre(Guid movieId, List<MovieGenreParam> genres);
        Task<MovieGenre> UpdateMovieGenre(MovieGenre movieGenre);
        Task<bool> DeleteMovieGenre(Guid id);
    }
}
