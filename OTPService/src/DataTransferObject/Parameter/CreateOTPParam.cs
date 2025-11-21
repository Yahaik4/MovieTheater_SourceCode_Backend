using Shared.Contracts.Constants;
using Shared.Contracts.Interfaces;

namespace OTPService.DataTransferObject.Parameter
{
    public class CreateOTPParam : IParam
    {
        public Guid UserId { get; set; }
        public string Purpose { get; set; } = OtpPurposeConstants.Register;
    }
}
