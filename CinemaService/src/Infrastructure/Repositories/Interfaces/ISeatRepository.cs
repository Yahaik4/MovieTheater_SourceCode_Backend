using src.Infrastructure.EF.Models;

namespace src.Infrastructure.Repositories.Interfaces
{
    public interface ISeatRepository
    {
        Task<Seat?> GetSeatById(Guid id);
        Task<IEnumerable<Seat>> GetAllSeatByRoom(Guid? RoomId);
        Task<Seat> CreateSeat(Seat seat);
        Task<Seat> UpdateSeat(Seat seat);
        Task<IEnumerable<Seat>> CreateSeats(List<Seat> seats);
        Task<bool> DeleteSeat(Seat seat);
    }
}
