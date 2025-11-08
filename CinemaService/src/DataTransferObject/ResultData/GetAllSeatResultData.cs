using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class GetAllSeatResultData : BaseResultData
    {
        public List<GetAllSeatDataResult> Data { get; set; }
    }

    public class GetAllSeatDataResult
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
