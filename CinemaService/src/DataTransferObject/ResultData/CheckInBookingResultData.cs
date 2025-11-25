using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class CheckInBookingResultData : BaseResultData
    {
        public CheckInBookingDataResult Data { get; set; }
    }

    public class CheckInBookingDataResult
    {
        public Guid BookingId { get; set; }
        public string Status { get; set; }
        public Guid CinemaId { get; set; }
        public string CinemaName { get; set; }
        public DateTime ShowtimeStartTime { get; set; }
        public DateTime ShowtimeEndTime { get; set; }
        public int NumberOfSeats { get; set; }
    }
}
