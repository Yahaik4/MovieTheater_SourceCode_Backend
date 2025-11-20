namespace ApiGateway.DataTransferObject.Parameter
{
    public class CreateBookingRequestParam
    {
        public Guid ShowtimeId { get; set; }
        public List<Guid> ShowtimeSeatIds { get; set; }
    }
}
