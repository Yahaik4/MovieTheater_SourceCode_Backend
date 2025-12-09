using Shared.Infrastructure;

namespace CinemaService.Infrastructure.EF.Models
{
    public class SpecialDayPricing : BaseEntity
    {
        public int Date { get; set; }
        public decimal ExtraPrice { get; set; }
        public string Label { get; set; } = string.Empty;
        public ICollection<Showtime> Showtimes { get; set; }
    }
}
