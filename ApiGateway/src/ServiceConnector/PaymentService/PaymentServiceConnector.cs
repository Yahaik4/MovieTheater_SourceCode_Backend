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

        public async Task<CreateTransactionGrpcReplyDTO> CreateTransaction(string userId, Guid bookingId, string currency, string paymentMethod)
        {
            using var channel = GetPaymentServiceChannel();
            var client = new PaymentGrpcService.PaymentGrpcServiceClient(channel);

            var request = new CreateTransactionGrpcRequestDTO
            {
                UserId = userId,
                BookingId = bookingId.ToString(),
                Currency = currency,
                PaymentMethod = paymentMethod

            };

            return await client.CreateTransactionAsync(request);
        }
    }
}
