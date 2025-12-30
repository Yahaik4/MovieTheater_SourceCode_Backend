using ApiGateway.DataTransferObject.Parameter;
using ApiGateway.DataTransferObject.ResultData;
using ApiGateway.ServiceConnector.PaymentService;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;

namespace ApiGateway.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentServiceConnector _paymentServiceConnector;

        public PaymentController(PaymentServiceConnector paymentServiceConnector)
        {
            _paymentServiceConnector = paymentServiceConnector;
        }

        private string GetIpAddress(HttpContext context)
        {
            try
            {
                var remoteIpAddress = context.Connection.RemoteIpAddress;

                if (remoteIpAddress != null)
                {
                    if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        remoteIpAddress = Dns.GetHostEntry(remoteIpAddress)
                            .AddressList
                            .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                    }

                    if (remoteIpAddress != null)
                    {
                        return remoteIpAddress.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "127.0.0.1";
        }

        [HttpPost("transaction")]
        public async Task<CreateTransactionResultDTO> CreateTransaction(
            CreateTransactionRequestParam param)
        {
            try
            {
                var userId =
                    User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                    ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

                string clientIp = GetIpAddress(HttpContext);

                var result = await _paymentServiceConnector.CreateTransaction(
                    userId,
                    param.BookingId,
                    param.PaymentGateway,
                    clientIp
                );

                return new CreateTransactionResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new CreateTransactionDataResult
                    {
                        TransactionId = result.Data.TransactionId,
                        PaymentIntentId = result.Data.PaymentIntentId,
                        ClientSecret = result.Data.ClientSecret,
                        Status = result.Data.Status,
                        Amount = decimal.Parse(result.Data.Amount),
                        Currency = result.Data.Currency,
                        CreatedAt = DateTime.Parse(result.Data.CreatedAt),
                        PaymentUrl = result.Data.PaymentUrl,
                        Provider = result.Data.Provider,
                        ProviderMeta = result.Data.ProviderMeta
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"CreateTransaction Error: {message}");

                return new CreateTransactionResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpGet("transaction/status")]
        public async Task<TransactionStatusResultDTO> GetTransactionStatus(
            [FromQuery] string txnRef)
        {
            var res = await _paymentServiceConnector.GetTransactionStatus(txnRef);

            return new TransactionStatusResultDTO
            {
                Result = res.Result,
                Message = res.Message,
                StatusCode = res.StatusCode,
                Data = res.Data == null
                    ? null
                    : new TransactionStatusData
                    {
                        TxnRef = res.Data.TxnRef,
                        Status = res.Data.Status,
                        UpdatedAt = DateTime.Parse(res.Data.UpdatedAt)
                    }
            };
        }

        [HttpGet("callback/vnpay")]
        public async Task<IActionResult> VnPayCallback(
            [FromQuery] HandleVnpayCallbackParam param)
        {
            var result = await _paymentServiceConnector.HanldeVnpayCallback(param);

            bool isSuccess =
                param.vnp_ResponseCode == "00"
                && param.vnp_TransactionStatus == "00";

            string title = isSuccess
                ? "Thanh toán thành công"
                : "Thanh toán thất bại";

            string message = isSuccess
                ? "Giao dịch đã được ghi nhận. Bạn có thể quay lại ứng dụng."
                : "Giao dịch không thành công. Vui lòng quay lại ứng dụng để thử lại.";

            string html = $@"
            <!DOCTYPE html>
            <html lang='vi'>
            <head>
                <meta charset='utf-8' />
                <title>{title}</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background: #0b1220;
                        color: #e5e7eb;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        height: 100vh;
                        margin: 0;
                    }}
                    .box {{
                        max-width: 420px;
                        padding: 24px;
                        background: rgba(255,255,255,0.06);
                        border-radius: 14px;
                        text-align: center;
                    }}
                    .title {{
                        font-size: 22px;
                        margin-bottom: 10px;
                    }}
                    .desc {{
                        opacity: 0.9;
                        margin-bottom: 14px;
                    }}
                    .countdown {{
                        font-size: 14px;
                        opacity: 0.7;
                    }}
                </style>
            </head>
            <body>
                <div class='box'>
                    <div class='title'>{title}</div>
                    <div class='desc'>{message}</div>
                    <div class='countdown'>
                        Trang sẽ tự đóng sau <b id='sec'>5</b> giây
                    </div>
                </div>

                <script>
                    let s = 5;
                    const el = document.getElementById('sec');

                    const timer = setInterval(() => {{
                        s--;
                        el.innerText = s;

                        if (s <= 0) {{
                            clearInterval(timer);
                            window.close();
                            setTimeout(() => {{
                                location.href = 'about:blank';
                            }}, 200);
                        }}
                    }}, 1000);
                </script>
            </body>
            </html>";

            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            return Content(html, "text/html; charset=utf-8");
        }

        // [HttpPost("callback/momo")]
        // public async Task<IActionResult> MomoCallback([FromBody] MomoCallbackRequest body)
        // {
        //     return Ok(); // momo yêu cầu trả về 200 OK
        // }

        // [HttpPost("callback/stripe")]
        // public async Task<IActionResult> StripeWebhook()
        // {
        //     using var reader = new StreamReader(Request.Body);
        //     var json = await reader.ReadToEndAsync();
        //     await _paymentClient.HandleStripeWebhook(json);
        //     return Ok();
        // }
    }
}
