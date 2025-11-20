using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBooking(Booking booking);
    }
}
