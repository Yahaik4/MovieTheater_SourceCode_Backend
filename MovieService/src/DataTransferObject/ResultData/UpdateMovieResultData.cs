using Shared.Contracts.ResultData;

namespace MovieService.DataTransferObject.ResultData
{
    public class UpdateMovieResultData : BaseResultData
    { 
        public UpdateMovieDataResult Data { get; set; }
    }

    public class UpdateMovieDataResult : MovieBaseResultData
    {
    }
}
