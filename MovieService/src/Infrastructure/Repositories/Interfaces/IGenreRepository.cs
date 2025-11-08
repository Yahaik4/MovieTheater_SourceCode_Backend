using MovieService.DataTransferObject.Parameter;
using MovieService.Infrastructure.EF.Models;

namespace MovieService.Infrastructure.Repositories.Interfaces
{
    public interface IGenreRepository
    {
        Task<IEnumerable<Genre>> GetGenres(GetGenresParam param);
        Task<Genre?> GetGenreById(Guid id);
        Task<IEnumerable<Genre>> GetGenreByIds(List<Guid> ids);
        Task<Genre?> GetGenreByName(string name);
        Task<Genre> CreateGenre(Genre genre);
        Task<Genre> UpdateGenre(Genre genre);
        Task<bool> DeleteGenre(Guid id);
    }
}
