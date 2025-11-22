using OTPService.Infrastructure.EF.Models;

namespace OTPService.Infrastructure.Repositories.Interfaces
{
    public interface IOTPRepository
    {
        Task<OTP?> GetOTPByUserAsync(Guid userId, string purpose);
        Task<OTP> CreateOrUpdateOTP(Guid userId, string code, TimeSpan expiry, string purpose);
        Task<bool> MarkOtpAsDeletedAsync(OTP otp);
    }
}
