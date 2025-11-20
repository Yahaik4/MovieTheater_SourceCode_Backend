namespace CinemaService.DataTransferObject.Parameter
{
    public class CreateBookingParam
    {
        public Guid UserId { get; set; }
        public Guid ShowtimeId { get; set; }
        public List<Guid> ShowtimeSeatIds { get; set; }
    }
}
