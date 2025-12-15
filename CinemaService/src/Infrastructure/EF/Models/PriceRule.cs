using Shared.Infrastructure;

namespace CinemaService.Infrastructure.EF.Models
{
    public class PriceRule : BaseEntity
    {
        public Guid CustomerTypeId { get; set; }
        public int DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public decimal BasePrice { get; set; }

        public CustomerType CustomerType { get; set; }
    }
}
