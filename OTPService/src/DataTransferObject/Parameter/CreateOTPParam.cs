using Shared.Contracts.Interfaces;

namespace OTPService.DataTransferObject.Parameter
{
    public class CreateOTPParam : IParam
    {
        public Guid UserId { get; set; }
    }
}
