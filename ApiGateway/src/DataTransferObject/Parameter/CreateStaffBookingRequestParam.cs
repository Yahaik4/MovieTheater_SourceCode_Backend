namespace ApiGateway.DataTransferObject.Parameter
{
    public class CreateStaffBookingRequestParam
    {
        public Guid? UserId { get; set; }
        public Guid ShowtimeId { get; set; }
        public Guid PromotionId { get; set; }
        public List<Guid> ShowtimeSeatIds { get; set; }
        public List<CreateBookingFoodDrinkRequestItem>? FoodDrinkItems { get; set; }
    }
}
