namespace src.DataTransferObject.ResultData
{
    public class RefreshTokenResultData : IResultData
    {
        public bool Result { get; set; }
        public string Message { get; set; } = null!;
        public RefreshTokenDataResult Data { get; set; } = null!;
    }

    public class RefreshTokenDataResult {
        public string AccessToken { get; set; }
    }
}
