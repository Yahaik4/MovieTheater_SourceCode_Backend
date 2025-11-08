using Shared.Contracts.Interfaces;
using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class GetAllCinemasResultData : BaseResultData
    {
        public List<GetAllCinemasDataResult> Data { get; set; }
    }

    public class GetAllCinemasDataResult : CinemaBaseDataResult
    {
        public int TotalRoom { get; set; }
    }
}
