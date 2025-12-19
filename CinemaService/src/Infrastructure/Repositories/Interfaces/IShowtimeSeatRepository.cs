using CinemaService.Infrastructure.EF.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface IShowtimeSeatRepository
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<List<ShowtimeSeat>> GetSeatsForBookingAsync(List<Guid> seatIds, Guid showtimeId);
        Task<IEnumerable<ShowtimeSeat>> GetShowtimeSeatsByShowtimeId(Guid showtimeId);
        Task<IEnumerable<ShowtimeSeat>> CreateShowtimeSeats(Guid showtimeId, List<Guid> seatIds, decimal basePriceRoom);
        Task<bool> DeleteShowtimeSeatsByShowtimeId(Guid showtimeId);
        Task UpdateSeatsAsync(List<ShowtimeSeat> seats);
        Task MarkSeatsAsAvailable(Guid showtimeId, List<Guid> seatIds);
    }
}
