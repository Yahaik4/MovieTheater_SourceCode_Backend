using src.DataTransferObject.Parameter;
using src.Infrastructure.EF.Models;

namespace src.Infrastructure.Repositories.Interfaces
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
