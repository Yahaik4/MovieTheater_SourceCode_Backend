namespace src.DataTransferObject.ResultData
{
    public class LoginResultData : IResultData
    {
        public bool Result {  get; set; }
        public string Message { get; set; } = null!;
        public LoginDataResult Data { get; set; } = null!;
    }

    public class LoginDataResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
