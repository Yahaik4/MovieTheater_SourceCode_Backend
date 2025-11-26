using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class GetBookingHistoryResultData : BaseResultData
    {
        public List<CreateBookingDataResult> Data { get; set; }
    }
}
