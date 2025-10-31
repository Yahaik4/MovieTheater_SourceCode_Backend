using src.Infrastructure.EF.Models;

namespace src.Infrastructure.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room?> GetRoomById(Guid roomId);
        Task<Room?> GetRoomByNumber(int number);
        Task<IEnumerable<Room>> GetAllRoomByCinema(Guid cinemaId, Guid? id, int? roomNumber, string? status, string? type);
        Task<Room> CreateRoom(Room room);
        Task<Room> UpdateRoom(Room room);
        Task<IEnumerable<Room>> AddListRoom(List<Room> rooms);
    }
}
