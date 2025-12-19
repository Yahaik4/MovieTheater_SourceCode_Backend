using Shared.Infrastructure;

namespace CinemaService.Infrastructure.EF.Models
{
    public class Booking : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ShowtimeId { get; set; }
        public Showtime Showtime { get; set; }
        public Guid? PromotionId { get; set; }
        public int NumberOfSeats { get; set; }
        public DateTime ExpiredAt { get; set; }
        public string Status { get; set; } // Pending | Paid | Cancelled | Expired | Failed
        public decimal TotalPrice { get; set; }

        public ICollection<ShowtimeSeat> ShowtimeSeats { get; set; }
        public List<BookingSeat> BookingSeats { get; set; }
        public ICollection<BookingItem> BookingItems { get; set; }
        public Promotion? Promotion { get; set; }
    }

    public class BookingSeat
    {
        public Guid SeatId { get; set; }
        public string SeatCode { get; set; }
        public string Label { get; set; }
        public decimal Price { get; set; }
        public string SeatType { get; set; }
    }
}
