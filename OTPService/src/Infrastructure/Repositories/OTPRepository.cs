using Microsoft.EntityFrameworkCore;
using OTPService.Data;
using OTPService.Infrastructure.EF.Models;
using OTPService.Infrastructure.Repositories.Interfaces;

namespace OTPService.Infrastructure.Repositories
{
    public class OTPRepository : IOTPRepository
    {
        private readonly OTPDbContext _context;

        public OTPRepository(OTPDbContext context)
        {
            _context = context;
        }

        public async Task<OTP> CreateOTP(Guid userId, string code, TimeSpan expiry)
        {
            var old = await _context.OTPs.FirstOrDefaultAsync(o => o.UserId == userId);

            if (old != null)
            {
                old.Code = code;
                old.ExpiryAt = DateTime.UtcNow.Add(expiry);

                _context.OTPs.Update(old);
                await _context.SaveChangesAsync();
                return old;
            }

            var otp = new OTP
            {
                UserId = userId,
                Code = code,
                ExpiryAt = DateTime.UtcNow.Add(expiry),
            };

            await _context.OTPs.AddAsync(otp);
            await _context.SaveChangesAsync();

            return otp;
        }


        public async Task<OTP?> GetOTPbyUserId(Guid userId)
        {
            return await _context.OTPs.FirstOrDefaultAsync(o => o.UserId == userId);
        }
    }
}
