using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using PaymentService.DataTransferObject.Parameter;
using PaymentService.DataTransferObject.ResultData;
using Shared.Contracts.Exceptions;

namespace PaymentService.Providers
{
    public class MomoPaymentProvider : IPaymentProvider
    {
        private readonly IConfiguration _configuration;

        public MomoPaymentProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<CreateTransactionDataResult> CreatePaymentAsync(BookingDataParam param)
        {
            if (param == null) throw new ValidationException("Parameter is null");
            if (param.UserId == Guid.Empty) throw new ValidationException("UserId cannot be empty GUID");
            if (param.BookingId == Guid.Empty) throw new ValidationException("BookingId cannot be empty GUID");

            // Lấy config
            string partnerCode = _configuration["MomoAPI:PartnerCode"];
            string accessKey = _configuration["MomoAPI:AccessKey"];
            string secretKey = _configuration["MomoAPI:SecretKey"];
            string returnUrl = _configuration["MomoAPI:ReturnUrl"];
            string notifyUrl = _configuration["MomoAPI:NotifyUrl"];
            string requestType = _configuration["MomoAPI:RequestType"];
            string momoUrl = _configuration["MomoAPI:MomoApiUrl"];

            // Tạo orderId và requestId
            string orderId = param.BookingId.ToString("N") + "-" + DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string requestId = orderId;

            // Amount (VND, nguyên giá trị, không dấu thập phân)
            string amount = ((long)param.Amount).ToString();

            // Order info: KHÔNG có space, KHÔNG có ký tự đặc biệt, format datetime chuẩn
            // Dùng ToString("N") cho GUID để bỏ dấu gạch ngang
            string movieNameStr = param.MovieName;
            string startTimeStr = param.StartTime;
            string endTimeStr = param.EndTime;

            string timeStr = $"{startTimeStr}-{endTimeStr}";

            string orderInfo = $"{movieNameStr} ({timeStr})";

            // extraData để trống
            string extraData = "";

            // Raw hash: THỨ TỰ CHÍNH XÁC như Momo yêu cầu (KHÔNG có requestType!)
            // Thứ tự: partnerCode -> accessKey -> requestId -> amount -> orderId -> orderInfo -> returnUrl -> notifyUrl -> extraData
            string rawHash = $"partnerCode={partnerCode}&accessKey={accessKey}&requestId={requestId}&amount={amount}&orderId={orderId}&orderInfo={orderInfo}&returnUrl={returnUrl}&notifyUrl={notifyUrl}&extraData={extraData}";

            string signature = HmacSHA256(rawHash, secretKey);

            var requestBody = new
            {
                partnerCode,
                accessKey,
                requestId,
                amount,
                orderId,
                orderInfo,
                returnUrl,
                notifyUrl,
                extraData,
                requestType,
                signature
            };

            using var client = new HttpClient();
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            // Log để debug (xóa khi production)
            Console.WriteLine($"Raw hash: {rawHash}");
            Console.WriteLine($"Signature: {signature}");
            Console.WriteLine($"Request body: {JsonSerializer.Serialize(requestBody)}");

            var response = await client.PostAsync(momoUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Momo response: {responseString}");

            var responseJson = JsonSerializer.Deserialize<JsonElement>(responseString);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Momo API error: " + responseString);
            }

            // Check resultCode thay vì chỉ check HTTP status
            if (responseJson.TryGetProperty("resultCode", out var resultCode) && resultCode.GetInt32() != 0)
            {
                throw new Exception("Momo API error: " + responseString);
            }

            if (!responseJson.TryGetProperty("payUrl", out var payUrlElement))
            {
                throw new Exception("Momo API response does not contain payUrl: " + responseString);
            }

            string payUrl = payUrlElement.GetString();

            return new CreateTransactionDataResult
            {
                TransactionId = orderId,
                Amount = param.Amount,
                Currency = "VND",
                Status = "pending",
                PaymentUrl = payUrl,
                Provider = "momo",
                ProviderMeta = responseString,
            };
        }

        private string HmacSHA256(string message, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            using var hmac = new HMACSHA256(keyBytes);
            var hashMessage = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            return BitConverter.ToString(hashMessage).Replace("-", "").ToLower();
        }
    }
}