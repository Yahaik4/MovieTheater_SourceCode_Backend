namespace NotificationService.DTOs
{
    public class PaymentEmailModel
    {
        public Guid BookingId { get; set; }
        public string MovieName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Cinema { get; set; }
        public string CinemaAddress { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public List<SeatInfo> Seats { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class SeatInfo
    {
        public string Label { get; set; }
        public string SeatType { get; set; }
    }

}
