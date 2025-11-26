using ApiGateway.DataTransferObject.Parameter;
using PaymentGrpc;

namespace ApiGateway.ServiceConnector.PaymentService
{
    public class PaymentServiceConnector : BaseServiceConnector
    {
        private readonly ServiceConnectorConfig _serviceConnectorConfig;

        public PaymentServiceConnector(IConfiguration configuration) : base(configuration)
        {
            _serviceConnectorConfig = GetServiceConnectorConfig();
        }

        public async Task<CreateTransactionGrpcReplyDTO> CreateTransaction(string userId, Guid bookingId, string paymentGateway, string clientIp)
        {
            using var channel = GetPaymentServiceChannel();
            var client = new PaymentGrpcService.PaymentGrpcServiceClient(channel);

            var request = new CreateTransactionGrpcRequestDTO
            {
                UserId = userId,
                BookingId = bookingId.ToString(),
                PaymentGateway = paymentGateway,
                ClientIp = clientIp

            };

            return await client.CreateTransactionAsync(request);
        }

        public async Task<HanldeVnpayCallbackGrpcReplyDTO> HanldeVnpayCallback(HandleVnpayCallbackParam param)
        {
            using var channel = GetPaymentServiceChannel();
            var client = new PaymentGrpcService.PaymentGrpcServiceClient(channel);

            var request = new HanldeVnpayCallbackGrpcRequestDTO
            {
                VnpAmount = param.vnp_Amount,
                VnpBankCode = param.vnp_BankCode,
                VnpBankTranNo = param.vnp_BankTranNo,
                VnpCardType = param.vnp_CardType,
                VnpOrderInfo = param.vnp_OrderInfo,
                VnpPayDate = param.vnp_PayDate,
                VnpResponseCode = param.vnp_ResponseCode,
                VnpSecureHash = param.vnp_SecureHash,
                VnpTmnCode = param.vnp_TmnCode,
                VnpTransactionNo = param.vnp_TransactionNo,
                VnpTransactionStatus = param.vnp_TransactionStatus,
                VnpTxnRef = param.vnp_TxnRef
            };

            return await client.HanldeVnpayCallbackAsync(request);
        }
    }
}
