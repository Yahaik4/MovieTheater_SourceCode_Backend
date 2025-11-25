using CinemaService.Data;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Booking> UpdateBooking(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking?> GetBookingById(Guid id)
        {
            return await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateExpiredBookingsStatusAsync()
        {
            const int batchSize = 500;
            bool hasMore = true;

            while (hasMore)
            {
                var expiredBookings = await _context.Bookings
                    .Where(b => b.Status == "pending" && b.ExpiredAt < DateTime.Now)
                    .Include(b => b.ShowtimeSeats)
                    .Take(batchSize)
                    .ToListAsync();

                if (expiredBookings.Count == 0)
                {
                    hasMore = false;
                    break;
                }

                foreach (var booking in expiredBookings)
                {
                    booking.Status = "expired";

                    foreach (var seat in booking.ShowtimeSeats)
                    {
                        seat.Status = "available";
                        seat.BookingId = null;
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<Booking?> GetBookingWithDetailsAsync(Guid bookingId)
        {
            return await _context.Bookings
                .Where(b => !b.IsDeleted && b.Id == bookingId)
                .Include(b => b.Showtime)
                    .ThenInclude(st => st.Room)
                        .ThenInclude(r => r.Cinema)
                .FirstOrDefaultAsync();
        }
    }
}
