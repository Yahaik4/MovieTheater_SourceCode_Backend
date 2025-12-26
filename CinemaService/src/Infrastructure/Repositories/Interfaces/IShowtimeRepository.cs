using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface IShowtimeRepository
    {
        Task<IEnumerable<Showtime>> GetShowtimesByRoomId(Guid roomId);
        Task<IEnumerable<Showtime>> GetShowtimesByRoomIdInDateRange(Guid roomId, DateOnly fromDate, DateOnly toDate);
        Task<Showtime?> GetShowtimeById (Guid id);
        Task<Showtime> CreateShowtime(Showtime showtime);
        Task<Showtime> UpdateShowtime(Showtime showtime);
        Task CompleteEndedShowtimesAsync();
        Task<IEnumerable<Showtime>> GetShowtimesByCinemaAndDate(Guid cinemaId, DateTime startDate, DateTime endDate);
    }
}
