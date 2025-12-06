namespace ApiGateway.DataTransferObject.ResultData
{
    public class GetAllShowtimesResultDTO : BaseResultDTO
    {
        public List<GetAllShowtimesCinemaResult> Data { get; set; } = new();
    }

    public class GetAllShowtimesCinemaResult
    {
        public Guid CinemaId { get; set; }
        public string CinemaName { get; set; }
        public string Address { get; set; }
        public List<GetAllShowtimesRoomTypeResult> RoomTypes { get; set; } = new();
    }

    public class GetAllShowtimesRoomTypeResult
    {
        public Guid RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public List<GetAllShowtimesShowtimeResult> Showtimes { get; set; } = new();
    }

    public class GetAllShowtimesShowtimeResult
    {
        public Guid ShowtimeId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public Guid MovieId { get; set; }
        public string? MovieName { get; set; }
    }
}
