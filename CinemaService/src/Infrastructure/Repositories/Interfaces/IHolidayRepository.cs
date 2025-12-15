using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface IHolidayRepository
    {
        Task<IEnumerable<Holiday>> GetHolidays(string? name, DateOnly? startDate, DateOnly? endDate);
        Task<Holiday?> GetHolidaysById(Guid id);
        Task<Holiday?> GetHolidayByDate(int day, int month);
        Task<Holiday> CreateHoliday(Holiday holiday);
        Task<Holiday> UpdateHoliday(Holiday holiday);
    }
}
