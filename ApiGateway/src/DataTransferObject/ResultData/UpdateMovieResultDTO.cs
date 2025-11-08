using Shared.Contracts.ResultData;

namespace ApiGateway.DataTransferObject.ResultData
{
    public class UpdateMovieResultDTO : BaseResultData
    {
        public UpdateMovieDataResult Data { get; set; }
    }

    public class UpdateMovieDataResult : MovieBaseResultData
    {
    }
}
