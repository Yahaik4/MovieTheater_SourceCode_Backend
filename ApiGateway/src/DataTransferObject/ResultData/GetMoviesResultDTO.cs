namespace ApiGateway.DataTransferObject.ResultData
{
    public class GetMoviesResultDTO : BaseResultDTO
    {
        public List<GetMoviesDataResult> Data { get; set; }
    }

    public class GetMoviesDataResult : MovieBaseResultData
    {
    }
}
