using NotificationService.DTOs;

namespace NotificationService.Templates
{
    public static class PaymentSuccessTemplate
    {
        public static string Generate(PaymentEmailModel model)
        {
            string seatsText = string.Join(", ",
                model.Seats.Select(s => $"{s.Label} ({s.SeatType})")
            );

            return $@"
<div style=""font-family: Arial, Helvetica, sans-serif; line-height: 1.6; color: #333;"">

    <h2 style=""text-align:center;"">🎉 Payment Successful</h2>

    <p>Thank you for your booking! Here are your ticket details:</p>

    <hr style=""border: none; border-top: 1px solid #ddd;"" />

    <p><strong>🎬 Movie:</strong> {model.MovieName}</p>

    <p><strong>🕒 Showtime:</strong> 
        {model.StartTime:HH:mm dd/MM/yyyy} – {model.EndTime:HH:mm}
    </p>

    <p><strong>🏢 Cinema:</strong> {model.Cinema}</p>
    <p><strong>📍 Address:</strong> {model.CinemaAddress}</p>

    <p><strong>🎟 Room:</strong> 
        {model.RoomNumber} ({model.RoomType})
    </p>

    <p><strong>💺 Seats:</strong> {seatsText}</p>

    <p><strong>💰 Total Price:</strong> 
        <span style=""color:#e53935; font-weight:bold;"">
            {model.TotalPrice:N0} VND
        </span>
    </p>

    <hr style=""border: none; border-top: 1px solid #ddd;"" />

    <!-- QR CODE CENTER -->
    <div style=""text-align:center; margin: 24px 0;"">
        <p style=""margin-bottom:12px; font-weight:bold;"">
            📱 Check-in QR Code
        </p>

        <img 
            src=""cid:qrcode"" 
            width=""220"" 
            style=""display:block; margin:0 auto;"" 
            alt=""QR Code"" 
        />

        <p style=""font-size:13px; color:#777; margin-top:12px;"">
            Please present this QR code at the cinema entrance
        </p>
    </div>

    <hr style=""border: none; border-top: 1px solid #ddd;"" />

    <p style=""text-align:center;"">
        Enjoy your movie 🍿<br/>
        <strong>Thank you for choosing us!</strong>
    </p>

</div>";
        }

    }

}
