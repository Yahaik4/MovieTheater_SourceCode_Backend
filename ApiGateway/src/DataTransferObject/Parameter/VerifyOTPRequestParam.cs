using Shared.Contracts.Enums;

namespace ApiGateway.DataTransferObject.Parameter
{
    public class VerifyOTPRequestParam
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
        public string Purpose { get; set; } = OtpPurposeConstants.Register;
    }
}
