using CinemaService.Data;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaService.Infrastructure.Repositories
{
    public class BookingItemRepository : IBookingItemRepository
    {
        private readonly CinemaDbContext _context;
        public BookingItemRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteBookingItemsByBooking(Guid bookingId)
        {
            var bookingItems = await _context.BookingItems
                .Where(b => b.BookingId == bookingId)
                .ToListAsync();

            if (!bookingItems.Any())
            {
                return false;
            }

            _context.BookingItems.RemoveRange(bookingItems);
            await _context.SaveChangesAsync();

            return true;
        }


        public Task<BookingItem> GetBookingItemById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BookingItem>> GetBookingItemsByBookingId(Guid bookingId)
        {
            return await _context.BookingItems.Where(b => b.BookingId == bookingId).ToListAsync();
        }

        public Task<IEnumerable<BookingItem>> UpdateBookigItem(BookingItem bookingItem)
        {
            throw new NotImplementedException();
        }
    }
}
