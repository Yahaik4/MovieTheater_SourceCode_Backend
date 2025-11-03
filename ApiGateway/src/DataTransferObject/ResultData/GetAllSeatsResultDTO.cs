namespace src.DataTransferObject.ResultData
{
    public class GetAllSeatsResultDTO : BaseResultDTO
    {
        public List<GetAllSeatsDataResult> Data { get; set; }
    }

    public class GetAllSeatsDataResult
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public int ColumnIndex { get; set; }
        public int DisplayNumber { get; set; }
        public string SeatCode { get; set; }
        public bool isActive { get; set; }
        public string Status { get; set; }
        public string SeatType { get; set; }
    }
}
