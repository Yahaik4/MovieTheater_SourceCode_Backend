using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class UpdateBookingResultData : BaseResultData
    {
        public UpdateBookingDataResult Data { get; set; }
    }

    public class UpdateBookingDataResult
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
