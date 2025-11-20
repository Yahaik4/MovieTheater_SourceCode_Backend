using OTPService.Infrastructure.EF.Models;

namespace OTPService.Infrastructure.Repositories.Interfaces
{
    public interface IOTPRepository
    {
        Task<OTP?> GetOTPbyUserId(Guid userId);
        Task<OTP> CreateOTP(Guid userId, string code, TimeSpan expiry);
    }
}
