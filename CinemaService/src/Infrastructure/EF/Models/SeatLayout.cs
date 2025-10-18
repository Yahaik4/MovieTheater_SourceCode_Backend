using Shared.Infrastructure;

namespace src.Infrastructure.EF.Models
{
    public class SeatLayout : BaseEntity
    {
        // Layout_10x10_NormalVIP
        public string Name { get; set; }
        public int total_rows { get; set; }
        public int total_columns { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<SeatLayoutDetail> SeatLayoutDetails { get; set; }
        public ICollection<ColumnSkip> ColumnSkips { get; set; }
    }
}
