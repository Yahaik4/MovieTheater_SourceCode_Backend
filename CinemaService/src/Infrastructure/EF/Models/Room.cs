using Shared.Infrastructure;

namespace src.Infrastructure.EF.Models
{
    public class Room : BaseEntity
    {
        public int RoomNumber { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public Guid CinemaId { get; set; }
        public Guid LayoutId { get; set; }
        public Cinema Cinema { get; set; }
        public SeatLayout SeatLayout { get; set; }
    }
}
