using CinemaService.Data;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaService.Infrastructure.Repositories
{
    public class SeatTypeRepository : ISeatTypeRepository
    {
        private readonly CinemaDbContext _context;

        public SeatTypeRepository(CinemaDbContext context)
        {
            _context = context;
        }
        
        public async Task<SeatType> CreateSeatType(SeatType seatType)
        {
            _context.SeatTypes.Add(seatType);
            await _context.SaveChangesAsync();
            return seatType;
        }

        public async Task<IEnumerable<SeatType>> GetAllSeatType(Guid? id, string? type, decimal? extraPice)
        {
            var query = _context.SeatTypes.AsQueryable();

            if (id.HasValue)
                query = query.Where(st => st.Id == id);

            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(st => st.Type.ToLower().Contains(type.ToLower()));

            if (extraPice.HasValue)
                query = query.Where(st => st.ExtraPrice == extraPice);

            query = query.Where(x => x.IsDeleted == false);

            return await query.ToListAsync();
        }

        public async Task<SeatType?> GetSeatTypeById(Guid id)
        {
            return await _context.SeatTypes.FirstOrDefaultAsync(st => st.Id == id);
        }

        public async Task<SeatType?> GetSeatTypeByName(string type)
        {
            return await _context.SeatTypes.FirstOrDefaultAsync(st => st.Type == type);
        }

        public async Task<SeatType> UpdateSeatType(SeatType seatType)
        {
            _context.SeatTypes.Update(seatType);
            await _context.SaveChangesAsync();
            return seatType;
        }
    }
}
