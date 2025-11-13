using Shared.Infrastructure;

namespace CinemaService.Infrastructure.EF.Models
{
    public class ShowtimeSeat : BaseEntity
    {
        public Guid ShowTimeId { get; set; }
        public Guid SeatId { get; set; }
        public string Status { get; set; }  // Available / Booked / Pending
        public Showtime Showtime { get; set; }
        public Seat Seat { get; set; }
    }
}
