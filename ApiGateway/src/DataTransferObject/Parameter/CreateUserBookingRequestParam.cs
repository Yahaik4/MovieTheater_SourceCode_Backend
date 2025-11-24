namespace ApiGateway.DataTransferObject.Parameter
{
    public class CreateUserBookingRequestParam
    {
        public Guid ShowtimeId { get; set; }
        public List<Guid> ShowtimeSeatIds { get; set; }
        public List<CreateBookingFoodDrinkRequestItem>? FoodDrinkItems { get; set; }
    }

    public class CreateBookingFoodDrinkRequestItem
    {
        public Guid FoodDrinkId { get; set; }
        public int Quantity { get; set; }
    }
}
