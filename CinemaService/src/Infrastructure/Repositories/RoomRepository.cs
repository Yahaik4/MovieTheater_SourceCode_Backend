using CinemaService.Data;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace CinemaService.Infrastructure.Repositories
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
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<IEnumerable<Room>> GetAllRoomByCinema(Guid cinemaId, Guid? id, int? roomNumber, string? status, string? type)
        {
            var query = _context.Rooms.Include(r => r.RoomType).AsQueryable();

            query = query.Where(r => r.CinemaId == cinemaId);

            if (id.HasValue)
                query = query.Where(r => r.Id == id);

            if (roomNumber.HasValue && roomNumber.Value > 0)
                query = query.Where(r => r.RoomNumber == roomNumber);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(r => r.Status.ToLower().Contains(status.ToLower()));

            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(x => x.RoomType.Type.ToLower().Contains(type.ToLower()));

            query = query.Where(x => x.IsDeleted == false);

            return await query.ToListAsync();
        }

        public async Task<Room?> GetRoomById(Guid roomId)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);
        }

        public async Task<Room> UpdateRoom(Room room)
        {
            _context.Rooms.Update(room);
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
