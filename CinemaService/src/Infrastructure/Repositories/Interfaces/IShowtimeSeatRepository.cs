using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface IShowtimeSeatRepository
    {
        Task<IEnumerable<ShowtimeSeat>> GetShowtimeSeatsByShowtimeId(Guid showtimeId);
        Task<IEnumerable<ShowtimeSeat>> CreateShowtimeSeats(Guid showtimeId, List<Guid> seatIds);
        Task<bool> DeleteShowtimeSeatsByShowtimeId(Guid showtimeId);
    }
}
