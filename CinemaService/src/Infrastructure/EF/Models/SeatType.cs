using Shared.Infrastructure;

namespace src.Infrastructure.EF.Models
{
    public class SeatType : BaseEntity
    {
        public string NameType { get; set; }
        public decimal BasePrice { get; set; }
        public ICollection<SeatLayoutDetail> SeatLayoutDetails { get; set; }
    }
}
