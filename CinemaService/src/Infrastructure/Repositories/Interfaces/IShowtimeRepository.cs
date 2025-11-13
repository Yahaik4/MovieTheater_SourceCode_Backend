using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface IShowtimeRepository
    {
        Task<IEnumerable<Showtime>> GetShowtimesByRoomId(Guid roomId);
        Task<Showtime?> GetShowtimeById (Guid id);
        Task<Showtime> CreateShowtime(Showtime showtime);
        Task<Showtime> UpdateShowtime(Showtime showtime);
    }
}
