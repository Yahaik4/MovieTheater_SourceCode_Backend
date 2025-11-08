using Shared.Infrastructure;

namespace CinemaService.Infrastructure.EF.Models
{
    public class RoomType : BaseEntity
    {
        public string Type { get; set; } // 2D, 3D, IMAX
        public decimal BasePrice { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}
