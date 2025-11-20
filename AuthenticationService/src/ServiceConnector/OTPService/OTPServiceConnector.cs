using OTPGrpc;

namespace AuthenticationService.ServiceConnector.ProfileService
{
    public class OTPServiceConnector : BaseServiceConnector
    {
        private readonly ServiceConnectorConfig _serviceConnectorConfig;

        public OTPServiceConnector(IConfiguration configuration) : base(configuration)
        {
            _serviceConnectorConfig = GetServiceConnectorConfig();
        }

        public async Task<CreateOTPGrpcReplyDTO> CreateOTP(Guid userId)
        {
            using var channel = GetOTPServiceChannel();
            var client = new OTPGrpcService.OTPGrpcServiceClient(channel);

            var request = new CreateOTPGrpcRequestDTO
            {
                UserId = userId.ToString(),
            };

            return await client.CreateOTPAsync(request);
        }
    }
}
