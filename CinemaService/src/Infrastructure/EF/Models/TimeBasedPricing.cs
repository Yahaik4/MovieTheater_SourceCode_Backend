using Shared.Infrastructure;

namespace CinemaService.Infrastructure.EF.Models
{
    public class TimeBasedPricing : BaseEntity
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public decimal ExtraPrice { get; set; }
        public string Description { get; set; } = string.Empty;
        public ICollection<Showtime> Showtimes { get; set; }
    }
}
