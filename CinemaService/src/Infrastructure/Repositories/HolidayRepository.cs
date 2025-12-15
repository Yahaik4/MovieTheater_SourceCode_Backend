using CinemaService.Data;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaService.Infrastructure.Repositories
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly CinemaDbContext _context;

        public HolidayRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<Holiday?> GetHolidayByDate(int day, int month)
        {
            return await _context.Holidays.FirstOrDefaultAsync(h => h.Day == day && h.Month == month);
        }

        public async Task<Holiday?> GetHolidaysById(Guid id)
        {
            return await _context.Holidays.FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<Holiday> CreateHoliday(Holiday holiday)
        {
            await _context.Holidays.AddAsync(holiday);
            _context.SaveChanges();
            return holiday;
        }

        public async Task<IEnumerable<Holiday>> GetHolidays(string? name, DateOnly? startDate, DateOnly? endDate)
        {
            var query = _context.Holidays.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(h => h.Name.Contains(name));
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                int sm = startDate.Value.Month;
                int sd = startDate.Value.Day;
                int em = endDate.Value.Month;
                int ed = endDate.Value.Day;

                if (sm < em || (sm == em && sd <= ed))
                {
                    query = query.Where(h =>
                        (h.Month > sm || (h.Month == sm && h.Day >= sd)) &&
                        (h.Month < em || (h.Month == em && h.Day <= ed))
                    );
                }
                else
                {
                    query = query.Where(h =>
                        (h.Month > sm || (h.Month == sm && h.Day >= sd)) ||
                        (h.Month < em || (h.Month == em && h.Day <= ed))
                    );
                }
            }

            return await query.ToListAsync();
        }

        public async Task<Holiday> UpdateHoliday(Holiday holiday)
        {
            _context.Holidays.Update(holiday);
            await _context.SaveChangesAsync();
            return holiday;
        }
    }
}
