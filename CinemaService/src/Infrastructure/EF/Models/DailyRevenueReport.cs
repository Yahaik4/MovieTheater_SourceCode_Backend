using Shared.Infrastructure;

namespace CinemaService.Infrastructure.EF.Models
{
    public class DailyRevenueReport : BaseEntity
    {
        public DateOnly Date { get; set; }
        public Guid CinemaId { get; set; }
        public decimal Sales { get; set; }
        public int TicketSold { get; set; }
        public decimal ProjectedProfit { get; set; }
        public Cinema Cinema { get; set; }
    }
}
