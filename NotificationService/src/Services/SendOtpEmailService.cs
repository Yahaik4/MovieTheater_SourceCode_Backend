using NotificationService.Helpers.cs;
using NotificationService.Templates;

namespace NotificationService.Services
{
    public class SendOtpEmailService : IEmailService
    {
        private readonly Mailer _mailer;

        public SendOtpEmailService(Mailer mailer)
        {
            _mailer = mailer;
        }

        public async Task<bool> SendOtpAsync(string email, string otp)
        {
            string httpContent = OTPTemplate.Generate(otp);
            await _mailer.SendMail(email, "Your OTP Code", httpContent);
            return true;
        }
    }
}
