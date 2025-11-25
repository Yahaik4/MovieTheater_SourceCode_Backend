namespace ApiGateway.DataTransferObject.ResultData
{
    public class CheckInBookingResultDTO : BaseResultDTO
    {
        public CheckInBookingDataResult? Data { get; set; }
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
