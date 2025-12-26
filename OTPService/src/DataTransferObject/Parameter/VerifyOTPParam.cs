using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace OTPService.DataTransferObject.Parameter
{
    public class VerifyOTPParam : IParam
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
        public string Purpose { get; set; } = OtpPurposeConstants.Register;
    }
}
