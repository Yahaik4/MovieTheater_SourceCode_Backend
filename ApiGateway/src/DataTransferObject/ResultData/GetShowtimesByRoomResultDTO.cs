namespace ApiGateway.DataTransferObject.ResultData
{
    public class GetShowtimesByRoomResultDTO : BaseResultDTO
    {
        public List<GetShowtimeByRoomDataResult> Data { get; set; }
    }

    public class GetShowtimeByRoomDataResult
    {
        public Guid ShowtimeId { get; set; }
        public string Status { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
    }
}
