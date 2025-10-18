using Shared.Infrastructure;

namespace src.Infrastructure.EF.Models
{
    public class SeatLayoutDetail : BaseEntity
    {
        public string name { get; set; }
        public string RowLabel { get; set; }
        public int ColumnNumber { get; set; }
        public int Column_start { get; set; }
        public Guid SeatLayoutId { get; set; }
        public Guid SeatTypeId { get; set; }
        public SeatLayout SeatLayout { get; set; }
        public SeatType SeatType { get; set; }
    }
}
