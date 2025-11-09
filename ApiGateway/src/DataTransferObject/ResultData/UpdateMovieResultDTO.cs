namespace ApiGateway.DataTransferObject.ResultData
{
    public class UpdateMovieResultDTO : BaseResultDTO
    {
        public UpdateMovieDataResult Data { get; set; }
    }

    public class UpdateMovieDataResult : MovieBaseResultData
    {
    }
}
