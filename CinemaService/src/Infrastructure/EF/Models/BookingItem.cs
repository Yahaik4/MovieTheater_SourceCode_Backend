using Shared.Infrastructure;

namespace CinemaService.Infrastructure.EF.Models
{
    public class BookingItem : BaseEntity
    {
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; }

        public Guid ItemId { get; set; }
        public FoodDrink FoodDrink { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
