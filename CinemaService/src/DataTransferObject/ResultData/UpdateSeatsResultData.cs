using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class UpdateSeatsResultData : BaseResultData
    {
        public List<UpdateSeatsDataResult> Data { get; set; } = null!;
    }

    public class UpdateSeatsDataResult
    {
        public string Label { get; set; }
        public int ColumnIndex { get; set; }
        public int DisplayNumber { get; set; }
        public string SeatCode { get; set; }
        public bool isActive { get; set; }
        public string Status { get; set; }
        public string SeatType { get; set; }
    }
}
