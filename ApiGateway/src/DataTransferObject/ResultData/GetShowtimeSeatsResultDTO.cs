namespace ApiGateway.DataTransferObject.ResultData
{
    public class GetShowtimeSeatsResultDTO : BaseResultDTO
    {
        public List<GetShowtimeSeatsDataResult> Data { get; set; }
    }

    public class GetShowtimeSeatsDataResult
    {
        public Guid SeatId { get; set; }
        public string SeatCode { get; set; }
        public string Label { get; set; }
        public string Status { get; set; }
    }
}
