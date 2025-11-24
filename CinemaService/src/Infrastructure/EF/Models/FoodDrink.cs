using Shared.Infrastructure;

namespace CinemaService.Infrastructure.EF.Models
{
    public class FoodDrink : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Size { get; set; } = null!;
        public decimal Price { get; set; }

        public ICollection<BookingItem> BookingItems { get; set; }
            = new List<BookingItem>();
    }
}
