using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface IBookingItemRepository
    {
        Task<BookingItem> GetBookingItemById(Guid id);
        Task<IEnumerable<BookingItem>> GetBookingItemsByBookingId(Guid bookingId);
        Task<IEnumerable<BookingItem>> UpdateBookigItem(BookingItem bookingItem);
        Task<bool> DeleteBookingItemsByBooking(Guid bookingId);
    }
}
