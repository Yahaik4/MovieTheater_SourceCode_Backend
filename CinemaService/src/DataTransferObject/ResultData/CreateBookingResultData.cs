using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class CreateBookingResultData : BaseResultData
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
    }

    public class BookingSeatsDataResult
    {
        public Guid SeatId { get; set; }
        public string SeatCode { get; set; }
        public string SeatType { get; set; }
        public string Label { get; set; }
        public decimal Price { get; set; }
    }
}
