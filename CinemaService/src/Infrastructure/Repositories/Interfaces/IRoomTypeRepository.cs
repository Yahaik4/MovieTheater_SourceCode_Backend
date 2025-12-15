using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface IRoomTypeRepository
    {
        Task<RoomType?> GetRoomTypeById(Guid id);
        Task<IEnumerable<RoomType>> GetAllRoomType(Guid? id, string? type, decimal? extraPrice);
        Task<RoomType> CreateRoomType(RoomType roomType);
        Task<RoomType> UpdateRoomType(RoomType roomType);
    }
}
