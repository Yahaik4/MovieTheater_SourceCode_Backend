using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.Infrastructure.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly CinemaDbContext _context;

        public SeatRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<Seat> CreateSeat(Seat seat)
        {
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();
            return seat;
        }

        public async Task<IEnumerable<Seat>> CreateSeats(List<Seat> seats)
        {
            await _context.Seats.AddRangeAsync(seats);
            await _context.SaveChangesAsync();
            return seats;
        }

        public async Task<IEnumerable<Seat>> GetAllSeatByRoom(Guid? RoomId)
        {
            return await _context.Seats.Where(s => s.RoomId == RoomId).ToListAsync();
        }

        public async Task<Seat?> GetSeatById(Guid id)
        {
            return await _context.Seats.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Seat> UpdateSeat(Seat seat)
        {
            _context.Seats.Update(seat);
            await _context.SaveChangesAsync(true);
            return seat;
        }

        public async Task<bool> DeleteSeat(Seat seat)
        {
            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
