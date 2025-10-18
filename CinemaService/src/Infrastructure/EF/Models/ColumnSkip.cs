using Shared.Infrastructure;

namespace src.Infrastructure.EF.Models
{
    public class ColumnSkip : BaseEntity
    {
        public int column { get; set; }
        public char start_label { get; set; }
        public char end_label { get; set; }
        public Guid SeatLayoutId { get; set; }
        public SeatLayout SeatLayout { get; set; }
    }
}
