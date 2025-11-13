namespace ApiGateway.DataTransferObject.ResultData
{
    public class CreateShowtimeResultDTO : BaseResultDTO
    { 
        public CreateShowtimeDataResult Data { get; set; } = null!;
    }

    public class CreateShowtimeDataResult
    {
        public Guid ShowtimeId { get; set; }
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public Guid RoomId { get; set; }
        public int RoomNumber { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Status { get; set; }
    }
}
