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
using System.Text.Json;

namespace ApiGateway.Controllers
{
    //[Authorize]
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

        [HttpPost("create-transaction")]
        public async Task<CreateTransactionResultDTO> CreateTransaction(CreateTransactionRequestParam param)
        {
            try
            {
                var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                     ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

                string clientIp = GetIpAddress(HttpContext);

                var result = await _paymentServiceConnector.CreateTransaction(userId, param.BookingId, param.PaymentGateway, clientIp);

                return new CreateTransactionResultDTO
                {
                    Result = result.Result,
                    Message = "Create Transaction successfully",
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
                        ProviderMeta = result.Data.ProviderMeta,
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new CreateTransactionResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpGet("callback/vnpay")]
        public async Task<IActionResult> VnPayCallback([FromQuery] HandleVnpayCallbackParam param)
        {
            var rawQuery = Request.QueryString.Value;
            Console.WriteLine("=== VNPAY CALLBACK RECEIVED ===");
            Console.WriteLine(rawQuery);

            Console.WriteLine(JsonSerializer.Serialize(param));

            var result = await _paymentServiceConnector.HanldeVnpayCallback(param);

            return Ok(result);
        }

        //[HttpPost("callback/momo")]
        //public async Task<IActionResult> MomoCallback([FromBody] MomoCallbackRequest body)
        //{
        //
        // await _paymentClient.HandleMomoCallbackAsync(body);
        //    return Ok(); // momo yêu cầu trả về 200 OK
        //}

        //[HttpPost("callback/stripe")]
        //public async Task<IActionResult> StripeWebhook()
        //{
        //    using var reader = new StreamReader(Request.Body);
        //    var json = await reader.ReadToEndAsync();

        //    await _paymentClient.HandleStripeWebhook(json);
        //    return Ok();
        //}
    }
}
