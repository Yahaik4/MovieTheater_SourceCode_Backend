using src.Infrastructure.EF.Models;

namespace src.Infrastructure.Repositories.Interfaces
{
    public interface ISeatTypeRepository
    {
        Task<SeatType?> GetSeatTypeById(Guid id);
        Task<IEnumerable<SeatType>> GetAllSeatType(Guid? id, string? type, decimal? extraPrice);
        Task<SeatType> CreateSeatType(SeatType seatType);
        Task<SeatType> UpdateSeatType(SeatType seatType);
        Task<SeatType?> GetSeatTypeByName(string type);
    }
}
