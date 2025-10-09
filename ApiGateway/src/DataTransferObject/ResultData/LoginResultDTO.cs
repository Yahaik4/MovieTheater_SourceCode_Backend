namespace src.DataTransferObject.ResultData
{
    public class LoginResultDTO : BaseResultDTO
    {
        public LoginDataResult? Data { get; set; }
    }

    public sealed class LoginDataResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
