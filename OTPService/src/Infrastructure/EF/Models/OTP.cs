using Shared.Contracts.Constants;
using Shared.Infrastructure;

namespace OTPService.Infrastructure.EF.Models
{
    public class OTP : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
        public DateTime ExpiryAt { get; set; }
        public string Purpose { get; set; } = OtpPurposeConstants.Register;
    }
}
