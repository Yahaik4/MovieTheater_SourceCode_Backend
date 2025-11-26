namespace PaymentService.DataTransferObject.Parameter
{
    public class BookingDataParam
    {
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public decimal Amount { get; set; }
        public string ClientIp { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
