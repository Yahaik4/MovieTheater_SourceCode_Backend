using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetBookingById(Guid id);
        Task<Booking> CreateBooking(Booking booking);
        Task<Booking> UpdateBooking(Booking booking);
        Task UpdateExpiredBookingsStatusAsync();
        Task<Booking?> GetBookingWithDetailsAsync(Guid bookingId);
    }
}
