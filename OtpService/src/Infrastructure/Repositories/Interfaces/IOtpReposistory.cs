namespace src.Infrastructure.Repositories.Interfaces
{
    public interface IOtpRepository
    {
        Task SaveOtpAsync(string userEmail, string Otp, TimeSpan expiry);
        Task<string> GetOtpAsync(string userEmail);
        Task DeleteOtpAsync(string userEmail);
        Task CleanupExpiredOtpsAsync();
    }
}