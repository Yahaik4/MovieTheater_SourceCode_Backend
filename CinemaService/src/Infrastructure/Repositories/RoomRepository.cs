using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly CinemaDbContext _context;

        public RoomRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<Room?> GetRoomByNumber(int number)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == number);
        }

        public async Task<Room> CreateRoom(Room room)
        {
            await _context.AddAsync(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<IEnumerable<Room>> GetAllRoom(Guid cinemaId)
        {
            return await _context.Rooms.Where(r => r.CinemaId == cinemaId).ToListAsync();
        }

        public async Task<Room?> GetRoomById(Guid roomId)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);
        }

        public async Task<Room> UpdateRoom(Room room)
        {
            _context.Update(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<IEnumerable<Room>> AddListRoom(List<Room> rooms)
        {
            await _context.Rooms.AddRangeAsync(rooms);
            await _context.SaveChangesAsync();
            return rooms;
        }
    }
}
