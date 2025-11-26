using PaymentService.DataTransferObject.ResultData;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace PaymentService.Libraries
{
    public class VnPayLibrary
    {
        private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
        private readonly SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());

        public CreateTransactionDataResult GetFullResponseData(IQueryCollection collection, string hashSecret)
        {
            var vnPay = new VnPayLibrary();

            // Lấy tất cả dữ liệu trả về từ VNPay
            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnPay.AddResponseData(key, value);
                }
            }

            // Lấy thông tin quan trọng
            var txnRef = vnPay.GetResponseData("vnp_TxnRef");               // TransactionId của bạn
            var vnpTranNo = vnPay.GetResponseData("vnp_TransactionNo");     // PaymentGatewayTransactionNo
            var responseCode = vnPay.GetResponseData("vnp_ResponseCode");   // Code trả về
            var orderInfo = vnPay.GetResponseData("vnp_OrderInfo");         // Mô tả order
            var amount = vnPay.GetResponseData("vnp_Amount");               // VND * 100
            var secureHash = collection.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value;

            // Check chữ ký
            var isValid = vnPay.ValidateSignature(secureHash, hashSecret);

            // Lấy thông tin phương thức thanh toán
            var cardType = vnPay.GetResponseData("vnp_CardType");           // loại thẻ / phương thức
            var bankCode = vnPay.GetResponseData("vnp_BankCode");           // ngân hàng

            // Build ProviderMeta
            var providerMeta = new
            {
                CardType = cardType,
                BankCode = bankCode
            };

            if (!isValid)
            {
                return new CreateTransactionDataResult
                {
                    TransactionId = txnRef,
                    Amount = amount != null ? decimal.Parse(amount) / 100 : 0,
                    Currency = "VND",
                    Status = "failed",  // lưu phương thức nếu có
                    Provider = "vnpay",
                    ProviderMeta = System.Text.Json.JsonSerializer.Serialize(providerMeta)
                };
            }

            // Status dựa vào responseCode: 00 = success, các mã khác = failed
            var status = responseCode == "00" ? "success" : "failed";

            return new CreateTransactionDataResult
            {
                TransactionId = txnRef,
                Amount = amount != null ? decimal.Parse(amount) / 100 : 0,
                Currency = "VND",
                Status = status,
                Provider = "vnpay",
                ProviderMeta = System.Text.Json.JsonSerializer.Serialize(providerMeta)
            };
        }


        public string GetIpAddress(HttpContext context)
        {
            var ipAddress = string.Empty;
            try
            {
                var remoteIpAddress = context.Connection.RemoteIpAddress;

                if (remoteIpAddress != null)
                {
                    if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                            .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                    }

                    if (remoteIpAddress != null) ipAddress = remoteIpAddress.ToString();

                    return ipAddress;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "127.0.0.1";
        }

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
        }
        public string CreateRequestUrl(string baseUrl, string vnpHashSecret)
        {
            var data = new StringBuilder();

            foreach (var (key, value) in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            var querystring = data.ToString();

            baseUrl += "?" + querystring;
            var signData = querystring;
            if (signData.Length > 0)
            {
                signData = signData.Remove(data.Length - 1, 1);
            }

            var vnpSecureHash = HmacSha512(vnpHashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnpSecureHash;

            return baseUrl;
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            var rspRaw = GetResponseData(); // dùng raw data
            var myChecksum = HmacSha512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }
        private string HmacSha512(string key, string inputData)
        {
            var hash = new StringBuilder();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

        private string GetResponseData()
        {
            var data = new StringBuilder();

            foreach (var kv in _responseData
                         .Where(kv => kv.Key != "vnp_SecureHash" && kv.Key != "vnp_SecureHashType")
                         .OrderBy(kv => kv.Key, StringComparer.Ordinal))
            {
                data.Append(kv.Key).Append("=").Append(kv.Value).Append("&");
            }

            if (data.Length > 0)
                data.Remove(data.Length - 1, 1); // bỏ & cuối

            return data.ToString();
        }
    }
}
public class VnPayCompare : IComparer<string>
{
    public int Compare(string x, string y)
    {
        if (x == y) return 0;
        if (x == null) return -1;
        if (y == null) return 1;
        var vnpCompare = CompareInfo.GetCompareInfo("en-US");
        return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
    }
}
