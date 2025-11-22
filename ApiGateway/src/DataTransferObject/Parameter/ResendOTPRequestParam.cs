using Shared.Contracts.Constants;

namespace ApiGateway.DataTransferObject.Parameter
{
    public class ResendOTPRequestParam
    {
        public required string email { get; set; }
        public string? Purpose { get; set; } = OtpPurposeConstants.Register;
    }
}