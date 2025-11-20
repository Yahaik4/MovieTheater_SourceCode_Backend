using Shared.Infrastructure;

namespace CinemaService.Infrastructure.EF.Models
{
    public class Showtime : BaseEntity
    {
        public Guid MovieId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }  // Scheduled // Open // Cancelled // Completed
        public Room Room { get; set; }
        public ICollection<ShowtimeSeat> ShowtimeSeats { get; set; }
        public ICollection<Booking> Bookings { get; set; }

    }
}
