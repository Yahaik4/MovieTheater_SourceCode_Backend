namespace ApiGateway.DataTransferObject.ResultData
{
    public class GetShowtimeDetailsResultDTO : BaseResultDTO
    {
        public GetShowtimeDetailsDataResult Data { get; set; }
    }

    public class GetShowtimeDetailsDataResult
    {
        public Guid CinemaId { get; set; }
        public string CinemaName { get; set; }
        public string City { get; set; }
        public Guid RoomId { get; set; }
        public int RoomNumber { get; set; }
        public int TotalColumn { get; set; }
        public int TotalRow { get; set; }
        public string RoomType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public string Poster { get; set; }
    }
}
