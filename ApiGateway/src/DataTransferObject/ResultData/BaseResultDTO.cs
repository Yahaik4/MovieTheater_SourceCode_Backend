namespace ApiGateway.DataTransferObject.ResultData
{
    public class BaseResultDTO
    {
        public bool Result {  get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
