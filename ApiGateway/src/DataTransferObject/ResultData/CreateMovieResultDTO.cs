namespace src.DataTransferObject.ResultData
{
    public class CreateMovieResultDTO : BaseResultDTO
    {
        public CreateMovieDataResult Data { get; set; }
    }

    public class CreateMovieDataResult : MovieBaseResultData
    {
    }
}
