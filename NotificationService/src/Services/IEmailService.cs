namespace NotificationService.Services
{
    public interface IEmailService
    {
        Task<bool> SendOtpAsync(string email, string otp);
    }
}
