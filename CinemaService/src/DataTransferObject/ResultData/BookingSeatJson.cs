namespace CinemaService.DataTransferObject.ResultData
{
    public class BookingSeatJson
    {
        public Guid SeatId { get; set; }
        public string Label { get; set; }
        public string SeatType { get; set; }
        public decimal Price { get; set; }
        public string SeatCode { get; set; }
    }

}
