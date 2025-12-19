namespace ApiGateway.DataTransferObject.ResultData
{
    public class UpdateBookingStatusResultDTO : BaseResultDTO
    {
        public UpdateBookingStatusDataResult Data { get; set; }
    }

    public class UpdateBookingStatusDataResult
    {
        public Guid BookingId { get; set; }
        public string Status { get; set; }
        public List<UpdateBookingSeatsDataResult> BookingSeats { get; set; }
    }

    public class UpdateBookingSeatsDataResult
    {
        public Guid SeatId { get; set; }
        public string SeatCode { get; set; }
        public string SeatType { get; set; }
        public string Label { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    }
}
