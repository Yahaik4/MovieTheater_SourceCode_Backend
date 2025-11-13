namespace ApiGateway.DataTransferObject.Parameter
{
    public class CreateShowtimeRequestParam
    {
        public Guid MovieId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
    }
}
