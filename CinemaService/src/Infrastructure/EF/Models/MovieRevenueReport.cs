using Shared.Infrastructure;

namespace CinemaService.Infrastructure.EF.Models
{
    public class MovieRevenueReport : BaseEntity
    {
        public DateTime Date { get; set; }
        public Guid CinemaId { get; set; }
        public Guid MovieId { get; set; }
        public decimal Sales { get; set; }
        public int TicketSold { get; set; }
        public decimal ProjectedProfit { get; set; }
        public Cinema Cinema { get; set; }
    }
}
