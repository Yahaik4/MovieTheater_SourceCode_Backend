using PaymentService.DataTransferObject.Parameter;
using PaymentService.DataTransferObject.ResultData;
using PaymentService.Libraries;
using PaymentService.ServiceConnector.CinemaService;

namespace PaymentService.Providers
{
    public class VnPayPaymentProvider : IPaymentProvider
    {
        private readonly IConfiguration _configuration;
        private readonly CinemaServiceConnector _cinemaServiceConnector;
        
    public VnPayPaymentProvider(IConfiguration configuration, CinemaServiceConnector cinemaServiceConnector)
        {
            _configuration = configuration;
            _cinemaServiceConnector = cinemaServiceConnector;
        }

        public async Task<CreateTransactionDataResult> CreatePaymentAsync(BookingDataParam param)
        {
            // Format số tiền cho VNPay (nhân 100)
            long vnpAmount = (long)param.Amount * 100;

            // Timezone
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);

            // Tạo txnRef duy nhất
            var txnRef = $"{param.BookingId:N}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["Vnpay:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", vnpAmount.ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", "VND");
            pay.AddRequestData("vnp_IpAddr", param.ClientIp);
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);

            // Mô tả order
            string movieNameStr = param.MovieName;
            string startTimeStr = param.StartTime;
            string endTimeStr = param.EndTime;

            string timeStr = $"{startTimeStr}-{endTimeStr}";

            string orderInfo = $"{movieNameStr} ({timeStr})";

            var expire = DateTime.Now.AddMinutes(15);
            // Format yyyymmddHHMMss
            string vnp_ExpireDate = expire.ToString("yyyyMMddHHmmss");

            //var orderInfo =
            //    $"Booking:{param.BookingId};Movie:{param.MovieId};Showtime:{param.StartTime:HH:mm dd/MM/yyyy} - {param.EndTime:HH:mm dd/MM/yyyy}";

            pay.AddRequestData("vnp_OrderInfo", orderInfo);
            pay.AddRequestData("vnp_OrderType", "billpayment");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_ExpireDate", vnp_ExpireDate);
            pay.AddRequestData("vnp_TxnRef", txnRef);

            // Tạo URL thanh toán
            var paymentUrl = pay.CreateRequestUrl(
                _configuration["Vnpay:BaseUrl"],
                _configuration["Vnpay:HashSecret"]
            );
            // Trả về CreateTransactionDataResult
            return new CreateTransactionDataResult
            {
                TransactionId = txnRef,
                Amount = param.Amount,
                Currency = "VND",
                Status = "pending",
                PaymentUrl = paymentUrl,
                Provider = "vnpay",
                ProviderMeta = null
            };
        }
    }
}
