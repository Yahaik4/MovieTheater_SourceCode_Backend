using Shared.Contracts.ResultData;

namespace MovieService.DataTransferObject.ResultData
{
    public class GetMoviesResultData : BaseResultData
    {
        public List<GetMoviesDataResult> Data { get; set; }
    }

    public class GetMoviesDataResult : MovieBaseResultData
    {
    }
}
