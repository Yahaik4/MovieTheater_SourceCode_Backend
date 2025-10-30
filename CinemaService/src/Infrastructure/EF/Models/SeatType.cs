using Shared.Infrastructure;

namespace src.Infrastructure.EF.Models
{
    public class SeatType : BaseEntity
    {
        public string Type { get; set; } // VIP, Couple, Standard.
        public decimal ExtraPrice { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }
}
