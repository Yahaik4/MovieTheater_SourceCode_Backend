namespace NotificationService.Helpers.cs
{
    using System.Net;
    using System.Net.Mail;
    using System.Net.Mime;

    public class Mailer
    {
        private readonly IConfiguration _configuration;

        public Mailer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendMail(string email, string sub, string content)
        {
            try
            {
                var fromEmail = new MailAddress(_configuration["MailSettings:Email"], "Your Display Name");
                var toEmail = new MailAddress(email);
                string password = _configuration["MailSettings:Password"];

                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(fromEmail.Address, password);

                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = fromEmail;
                        mailMessage.To.Add(toEmail);
                        mailMessage.Subject = sub;
                        mailMessage.Body = content;
                        mailMessage.IsBodyHtml = true;

                        await client.SendMailAsync(mailMessage);
                    }
                }
                return true;
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
                return false;
                // throw new Exception("Lỗi gửi email, vui lòng kiểm tra cấu hình SMTP.", smtpEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                return false;
                // throw new Exception("Lỗi gửi email, vui lòng kiểm tra cấu hình SMTP.");
            }
        }

        public async Task<bool> SendMailWithInlineImage(string email,
                                                        string subject,
                                                        string htmlContent,
                                                        byte[] imageBytes
                                                        )
        {
            var fromEmail = new MailAddress(_configuration["MailSettings:Email"], "Your Display Name");
            var toEmail = new MailAddress(email);
            string password = _configuration["MailSettings:Password"];

            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(fromEmail.Address, password)
            };

            using var mailMessage = new MailMessage
            {
                From = fromEmail,
                Subject = subject,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            // HTML view
            var htmlView = AlternateView.CreateAlternateViewFromString(
                htmlContent,
                null,
                MediaTypeNames.Text.Html
            );

            // Inline QR
            var qrImage = new LinkedResource(
                new MemoryStream(imageBytes),
                MediaTypeNames.Image.Png
            )
            {
                ContentId = "qrcode",
                TransferEncoding = TransferEncoding.Base64
            };

            htmlView.LinkedResources.Add(qrImage);
            mailMessage.AlternateViews.Add(htmlView);

            await client.SendMailAsync(mailMessage);
            return true;
        }
    }
}
