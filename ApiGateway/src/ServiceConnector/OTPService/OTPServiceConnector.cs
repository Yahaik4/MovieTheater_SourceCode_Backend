using ApiGateway.DataTransferObject.Parameter;
using ApiGateway.Helper;
using OTPGrpc;
using Shared.Contracts.Enums;

namespace ApiGateway.ServiceConnector.OTPService
{
    public class OTPServiceConnector : BaseServiceConnector
    {
        private readonly ServiceConnectorConfig _serviceConnectorConfig;

        public OTPServiceConnector(IConfiguration configuration) : base(configuration)
        {
            _serviceConnectorConfig = GetServiceConnectorConfig();
        }

        public async Task<CreateOTPGrpcReplyDTO> CreateOTP(Guid userId, string purpose)
        {
            using var channel = GetOTPServiceChannel();
            var client = new OTPGrpcService.OTPGrpcServiceClient(channel);

            var normalizedPurpose = string.IsNullOrWhiteSpace(purpose)
                ? OtpPurposeConstants.Register
                : purpose.ToLowerInvariant();

            var request = new CreateOTPGrpcRequestDTO
            {
                UserId = userId.ToString(),
                Purpose = normalizedPurpose
            };

            return await client.CreateOTPAsync(request);
        }

        public async Task<VerifyOTPGrpcReplyDTO> VerifyOTP(Guid userId, string code, string purpose)
        {
            using var channel = GetOTPServiceChannel();
            var client = new OTPGrpcService.OTPGrpcServiceClient(channel);

            var normalizedPurpose = string.IsNullOrWhiteSpace(purpose)
                ? OtpPurposeConstants.Register
                : purpose.ToLowerInvariant();

            var request = new VerifyOTPGrpcRequestDTO
            {
                UserId = userId.ToString(),
                Code = code,
                Purpose = normalizedPurpose
            };

            return await client.VerifyOTPAsync(request);
        }


    }
}
