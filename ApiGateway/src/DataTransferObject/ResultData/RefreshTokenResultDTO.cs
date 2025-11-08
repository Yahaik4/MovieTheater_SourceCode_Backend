namespace ApiGateway.DataTransferObject.ResultData
{
    public class RefreshTokenResultDTO : BaseResultDTO
    {
        public RefreshTokenDataResult? Data { get; set; }
    }

    public sealed class RefreshTokenDataResult
    {
        public string AccessToken { get; set; }
    }
}
