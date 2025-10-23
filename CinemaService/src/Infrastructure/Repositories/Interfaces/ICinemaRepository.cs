using src.Infrastructure.EF.Models;

namespace src.Infrastructure.Repositories.Interfaces
{
    public interface ICinemaRepository
    {
        Task<IEnumerable<Cinema>> GetAllCinema(Guid? id, string? name, string? city, string? status);
        Task<Cinema?> GetCinemaById(Guid cinemaId);
        Task<Cinema> CreateCinema(Cinema cinema);
        Task<Cinema> UpdateCinema(Cinema cinema);
    }
}
