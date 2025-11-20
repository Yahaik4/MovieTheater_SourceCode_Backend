namespace ApiGateway.DataTransferObject.Parameter
{
    public class VerifyOTPRequestParam
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
    }
}
