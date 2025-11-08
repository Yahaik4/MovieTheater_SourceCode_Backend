using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface ISeatRepository
    {
        Task<Seat?> GetSeatById(Guid id);
        Task<IEnumerable<Seat>> GetSeatByIds(List<Guid> Ids);
        Task<IEnumerable<Seat>> GetSeatsByIds(Guid[] ids);
        Task<IEnumerable<Seat>> GetAllSeatByRoom(Guid? RoomId);
        Task<Seat> CreateSeat(Seat seat);
        Task<Seat> UpdateSeat(Seat seat);
        Task<IEnumerable<Seat>> CreateSeats(List<Seat> seats);
        Task<bool> DeleteSeats(List<Seat> seats);
        Task UpdateSeats(IEnumerable<Seat> seats);
    }
}
