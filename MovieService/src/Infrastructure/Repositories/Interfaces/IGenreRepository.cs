using src.DataTransferObject.Parameter;
using src.Infrastructure.EF.Models;

namespace src.Infrastructure.Repositories.Interfaces
{
    public interface IGenreRepository
    {
        Task<IEnumerable<Genre>> GetGenres(GetGenresParam param);
        Task<Genre> CreateGenre(Genre genre);
        Task<Genre> UpdateGenre(Genre genre);
        Task<bool> DeleteGenre(Guid id);
    }
}
