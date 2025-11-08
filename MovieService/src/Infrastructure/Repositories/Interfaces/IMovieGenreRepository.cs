using MovieService.DataTransferObject.Parameter;
using MovieService.Infrastructure.EF.Models;

namespace MovieService.Infrastructure.Repositories.Interfaces
{
    public interface IMovieGenreRepository
    {
        //Task<IEnumerable<Genre>> GetGenres(GetGenresParam param);
        Task<IEnumerable<MovieGenre>> CreateMovieGenre(Guid movieId, List<MovieGenreParam> genres);
        Task<MovieGenre> UpdateMovieGenre(MovieGenre movieGenre);
        Task<bool> DeleteMovieGenre(Guid id);
    }
}
