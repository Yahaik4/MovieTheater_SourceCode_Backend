using Shared.Contracts.ResultData;

namespace src.DataTransferObject.ResultData
{
    public class CreateMovieResultData : BaseResultData
    {
        public CreateMovieDataResult Data { get; set; }
    }

    public class CreateMovieDataResult : MovieBaseResultData 
    {
    }
}
