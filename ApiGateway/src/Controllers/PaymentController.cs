using ApiGateway.DataTransferObject.Parameter;
using ApiGateway.DataTransferObject.ResultData;
using ApiGateway.ServiceConnector.PaymentService;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Utils;
using System.IdentityModel.Tokens.Jwt;
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

        [HttpPost("payment-intent")]
        public async Task<CreateTransactionResultDTO> CreateTransaction(CreateTransactionRequestParam param)
        {
            try
            {
                var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                     ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _paymentServiceConnector.CreateTransaction(userId, param.BookingId, param.Currency, param.PaymentMethod);

                return new CreateTransactionResultDTO
                {
                    Result = result.Result,
                    Message = "Create Transaction successfully",
                    StatusCode = result.StatusCode,
                    Data = new CreateTransactionDataResult
                    {
                        TransactionId = Guid.Parse(result.Data.TransactionId),
                        PaymentIntentId = result.Data.PaymentIntentId,
                        ClientSecret = result.Data.ClientSecret,
                        Status = result.Data.Status,
                        Amount = decimal.Parse(result.Data.Amount),
                        Currency = result.Data.Currency,
                        CreatedAt = DateTime.Parse(result.Data.CreatedAt)
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
    }
}
