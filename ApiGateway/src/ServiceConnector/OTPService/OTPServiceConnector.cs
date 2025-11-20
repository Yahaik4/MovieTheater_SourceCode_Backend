using ApiGateway.DataTransferObject.Parameter;
using ApiGateway.Helper;
using OTPGrpc;

namespace ApiGateway.ServiceConnector.OTPService
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

        public async Task<VerifyOTPGrpcReplyDTO> VerifyOTP(Guid userId, string code)
        {
            using var channel = GetOTPServiceChannel();
            var client = new OTPGrpcService.OTPGrpcServiceClient(channel);

            var request = new VerifyOTPGrpcRequestDTO
            {
                UserId = userId.ToString(),
                Code = code
            };

            return await client.VerifyOTPAsync(request);
        }
    }
}
