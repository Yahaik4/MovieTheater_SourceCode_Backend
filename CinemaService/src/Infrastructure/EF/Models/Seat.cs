using Shared.Infrastructure;

namespace CinemaService.Infrastructure.EF.Models
{
    public class Seat : BaseEntity
    {
        public string Label { get; set; }
        public int ColumnIndex { get; set; }
        public int DisplayNumber { get; set; }
        public string SeatCode { get; set; }
        public bool isActive { get; set; } = true;
        public string Status { get; set; }
        public Guid SeatTypeId { get; set; }
        public Guid RoomId { get; set; }
        public SeatType SeatType { get; set; }
        public Room Room { get; set; }
        public ICollection<ShowtimeSeat> ShowtimeSeats { get; set; }

    }
}
