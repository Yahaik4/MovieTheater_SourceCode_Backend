using Shared.Infrastructure;

namespace CinemaService.Infrastructure.EF.Models
{
    public class Room : BaseEntity
    {
        public int RoomNumber { get; set; }
        public string Status { get; set; } // 
        public int Total_Column { get; set; }
        public int Total_Row { get; set; }
        public Guid RoomTypeId { get; set; }
        public Guid CinemaId { get; set; }
        public RoomType RoomType { get; set; }
        public Cinema Cinema { get; set; }
        public ICollection<Seat> Seats { get; set; }
        public ICollection<Showtime> Showtimes { get; set; }
    }
}
