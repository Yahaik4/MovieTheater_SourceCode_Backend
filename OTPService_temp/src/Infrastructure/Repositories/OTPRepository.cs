using Microsoft.EntityFrameworkCore;
using OTPService.Data;
using OTPService.Infrastructure.EF.Models;
using OTPService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Constants;

namespace OTPService.Infrastructure.Repositories
{
    public class OTPRepository : IOTPRepository
    {
        private readonly OTPDbContext _context;

        public OTPRepository(OTPDbContext context)
        {
            _context = context;
        }

        public async Task<OTP> CreateOrUpdateOTP(Guid userId, string code, TimeSpan expiry, string purpose)
        {
            var normalizedPurpose = purpose.ToLowerInvariant();
            var old = await _context.OTPs.FirstOrDefaultAsync(o => o.UserId == userId && o.Purpose == normalizedPurpose);

            if (old != null)
            {
                old.Code = code;
                old.ExpiryAt = DateTime.UtcNow.Add(expiry);
                old.IsDeleted = false;
                old.Purpose = normalizedPurpose;
                old.UpdatedAt = DateTime.UtcNow;

                _context.OTPs.Update(old);
                await _context.SaveChangesAsync();
                return old;
            }

            var otp = new OTP
            {
                UserId = userId,
                Code = code,
                ExpiryAt = DateTime.UtcNow.Add(expiry),
                Purpose = normalizedPurpose,
            };

            await _context.OTPs.AddAsync(otp);
            await _context.SaveChangesAsync();

            return otp;
        }


        public async Task<OTP?> GetOTPByUserAsync(Guid userId, string purpose)
        {
            var normalizedPurpose = purpose.ToLowerInvariant();
            return await _context.OTPs.FirstOrDefaultAsync(o => o.UserId == userId && o.Purpose == normalizedPurpose && o.IsDeleted == false);
        }

        public async Task<bool> MarkOtpAsDeletedAsync(OTP otp)
        {
            otp.IsDeleted = true;
            otp.UpdatedAt = DateTime.UtcNow;

            _context.OTPs.Update(otp);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
