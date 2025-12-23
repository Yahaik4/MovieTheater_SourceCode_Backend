using NotificationService.DTOs;
using NotificationService.Helpers.cs;
using NotificationService.ServiceConnector.AuthenticationService;
using NotificationService.ServiceConnector.CinemaService;
using NotificationService.Templates;
using QRCoder.Core;
using Shared.Contracts.Exceptions;

namespace NotificationService.Services
{
    public class SendOtpEmailService : IEmailService
    {
        private readonly Mailer _mailer;
        private readonly AuthenticationServiceConnector _authenticationServiceConnector;
        private readonly CinemaServiceConnector _cinemaServiceConnector;

        public SendOtpEmailService(Mailer mailer, AuthenticationServiceConnector authenticationServiceConnector, CinemaServiceConnector cinemaServiceConnector)
        {
            _mailer = mailer;
            _authenticationServiceConnector = authenticationServiceConnector;
            _cinemaServiceConnector = cinemaServiceConnector;
        }

        public async Task<bool> SendEmailPaymentSuccess(Guid userId,Guid bookingId)
        {
            var email = await _authenticationServiceConnector.GetEmail(userId);

            if (!email.Result)
            {
                throw new ValidationException("Email not found");
            }

            var qrGenerator = new QRCodeGenerator();
            var qrData = qrGenerator.CreateQrCode(
                bookingId.ToString(),
                QRCodeGenerator.ECCLevel.Q
            );

            var qrCode = new PngByteQRCode(qrData);
            byte[] qrBytes = qrCode.GetGraphic(20);

            var bookingInfo = await _cinemaServiceConnector.GetBooking(bookingId);

            if (bookingInfo == null || !bookingInfo.Result || bookingInfo.Data == null)
            {
                throw new Exception("Booking information not found");
            }

            var paymentEmailModel = new PaymentEmailModel
            {
                BookingId = bookingId,

                MovieName = bookingInfo.Data.MovieName,
                StartTime = bookingInfo.Data.StartTime,
                EndTime = bookingInfo.Data.EndTime,
                Cinema = bookingInfo.Data.Cinema,
                CinemaAddress = bookingInfo.Data.Address,
                RoomNumber = bookingInfo.Data.RoomNumber,
                RoomType = bookingInfo.Data.RoomType,
                TotalPrice = decimal.Parse(bookingInfo.Data.TotalPrice),
                Seats = bookingInfo.Data.Seats.Select(s => new SeatInfo
                {
                    Label = s.Label,
                    SeatType = s.SeatType
                }).ToList()
            };

            string htmlContent = PaymentSuccessTemplate.Generate(paymentEmailModel);

            // Send mail with inline QR
            await _mailer.SendMailWithInlineImage(
                email.Data.Email,
                "Payment Successful - Your QR Code",
                htmlContent,
                qrBytes
            );

            return true;
        }

        public async Task<bool> SendOtpAsync(string email, string otp)
        {
            string httpContent = OTPTemplate.Generate(otp);
            await _mailer.SendMail(email, "Your OTP Code", httpContent);
            return true;
        }
    }
}
