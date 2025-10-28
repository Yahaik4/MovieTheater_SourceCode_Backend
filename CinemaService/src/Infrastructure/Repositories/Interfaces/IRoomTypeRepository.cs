using src.Infrastructure.EF.Models;

namespace src.Infrastructure.Repositories.Interfaces
{
    public interface IRoomTypeRepository
    {
        Task<RoomType?> GetRoomTypeById(Guid id);
        Task<IEnumerable<RoomType>> GetAllRoomType(Guid? id, string? type, decimal? basePrice);
        Task<RoomType> CreateRoomType(RoomType roomType);
        Task<RoomType> UpdateRoomType(RoomType roomType);
    }
}
