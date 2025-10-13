namespace src.DataTransferObject.ResultData
{
    public class LogoutResultData : IResultData
    {
        public bool Result { get; set; }
        public string Message { get; set; } = null!;
    }
}
