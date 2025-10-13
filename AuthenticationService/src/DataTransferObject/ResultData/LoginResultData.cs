namespace src.DataTransferObject.ResultData
{
    public class LoginResultData : BaseResultData
    {
        public LoginDataResult Data { get; set; } = null!;
    }

    public class LoginDataResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
