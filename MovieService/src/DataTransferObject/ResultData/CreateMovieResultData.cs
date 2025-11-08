using Shared.Contracts.ResultData;

namespace MovieService.DataTransferObject.ResultData
{
    public class CreateMovieResultData : BaseResultData
    {
        public CreateMovieDataResult Data { get; set; }
    }

    public class CreateMovieDataResult : MovieBaseResultData 
    {
    }
}
