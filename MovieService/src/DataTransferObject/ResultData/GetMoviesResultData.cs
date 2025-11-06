using Shared.Contracts.ResultData;

namespace src.DataTransferObject.ResultData
{
    public class GetMoviesResultData : BaseResultData
    {
        public List<GetMoviesDataResult> Data { get; set; }
    }

    public class GetMoviesDataResult : MovieBaseResultData
    {
    }
}
