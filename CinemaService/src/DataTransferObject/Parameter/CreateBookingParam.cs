namespace CinemaService.DataTransferObject.Parameter
{
    public class CreateBookingParam
    {
        public Guid UserId { get; set; }
        public Guid ShowtimeId { get; set; }
        public Guid? PromotionId { get; set; }
        public List<Guid> ShowtimeSeatIds { get; set; }
        public List<CreateBookingFoodDrinkItemParam>? FoodDrinkItems { get; set; }
    }

    public class CreateBookingFoodDrinkItemParam
    {
        public Guid FoodDrinkId { get; set; }
        public int Quantity { get; set; }
    }
}
