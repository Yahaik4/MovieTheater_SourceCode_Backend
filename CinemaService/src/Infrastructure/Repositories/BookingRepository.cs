using CinemaService.Data;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;

namespace CinemaService.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly CinemaDbContext _context;

        public BookingRepository(CinemaDbContext context)
        {
            _context = context;
        }
        public async Task<Booking> CreateBooking(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }
    }
}
