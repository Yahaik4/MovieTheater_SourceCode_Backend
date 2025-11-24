namespace ApiGateway.DataTransferObject.ResultData
{
    public class CreateBookingResultDTO : BaseResultDTO
    {
        public CreateBookingDataResult Data { get; set; }
    }

    public class CreateBookingDataResult
    {
        public Guid BookingId { get; set; }
        public string CinemaName { get; set; }
        public string MovieName { get; set; }
        public int RoomNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NumberOfSeats { get; set; }
        public decimal TotalPrice { get; set; }
        public List<BookingSeatsDataResult> BookingSeats { get; set; }

        public List<BookingFoodDrinkDataResult> BookingFoodDrinks { get; set; }
    }

    public class BookingSeatsDataResult
    {
        public Guid SeatId { get; set; }
        public string SeatCode { get; set; }
        public string SeatType { get; set; }
        public string Label { get; set; }
        public decimal Price { get; set; }
    }

    // NEW
    public class BookingFoodDrinkDataResult
    {
        public Guid FoodDrinkId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
