using Microsoft.EntityFrameworkCore;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.Infrastructure.Repositories;

public class OtpRepository : IOtpRepository
{
    private readonly OtpDbContext _context;

    public OtpRepository(OtpDbContext context)
    {
        _context = context;
    }

    public async Task SaveOtpAsync(string email, string otpCode, TimeSpan expiry)
    {
        var now = DateTime.UtcNow;

        var oldOtps = await _context.Otps
                .Where(o => o.UserEmail == email && !o.IsDeleted)
                .ToListAsync();

        if (oldOtps.Any())
        {
            _context.Otps.RemoveRange(oldOtps);
        }

        var newOtp = new Otp
        {
            Id = Guid.NewGuid(),
            UserEmail = email,
            Code = otpCode,
            ExpiryAt = now.Add(expiry),
        };
        
        await _context.Otps.AddAsync(newOtp);
        await _context.SaveChangesAsync();
    }

    public async Task<string> GetOtpAsync(string email)
    {
        var now = DateTime.UtcNow;

        var otp = await _context.Otps
            .Where(o => o.UserEmail == email && !o.IsDeleted && o.ExpiryAt > now)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        return otp?.Code ?? string.Empty;
    }

    public async Task DeleteOtpAsync(string email)
    {
        var otps = await _context.Otps
            .Where(o => o.UserEmail == email && !o.IsDeleted)
            .ToListAsync();

        if (otps.Any())
        {
            _context.Otps.RemoveRange(otps);
            await _context.SaveChangesAsync();
        }
    }

    public async Task CleanupExpiredOtpsAsync()
    {
        var expired = await _context.Otps
            .Where(o => o.ExpiryAt < DateTime.UtcNow)
            .ToListAsync();

        if (expired.Any())
        {
            _context.Otps.RemoveRange(expired);
            await _context.SaveChangesAsync();
        }
    }
}