using Shared.Infrastructure;

namespace CinemaService.Infrastructure.EF.Models
{
    public class FoodDrinkRevenueReport : BaseEntity
    {
        public DateOnly Date { get; set; }
        public Guid CinemaId { get; set; }
        public Guid FoodDrinkId { get; set; }
        public decimal Sales { get; set; }
        public int Quantity { get; set; }
        public decimal ProjectedProfit { get; set; }
        public Cinema Cinema { get; set; }
        public FoodDrink FoodDrink { get; set; }
    }
}
