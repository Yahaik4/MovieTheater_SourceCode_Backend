namespace ApiGateway.DataTransferObject.ResultData
{
    public class ResendOTPResultDTO : BaseResultDTO
    {
        public ResendOTPDataResult? Data { get; set; }
    }

    public class ResendOTPDataResult
    {
        public string Code { get; set; } = string.Empty;
        public string Expiry { get; set; } = string.Empty;
    }
}