using AuthenticationGrpc;

namespace NotificationService.ServiceConnector.AuthenticationService
{
    public class AuthenticationServiceConnector : BaseServiceConnector
    {
        private readonly ServiceConnectorConfig _serviceConnectorConfig;

        public AuthenticationServiceConnector(IConfiguration configuration) : base(configuration)
        {
            _serviceConnectorConfig = GetServiceConnectorConfig();
        }

        public async Task<GetEmailGrpcReplyDTO> GetEmail(Guid userId)
        {
            using var channel = GetAuthenticationServiceChannel();
            var client = new AuthenticationGrpcService.AuthenticationGrpcServiceClient(channel);

            var request = new GetEmailGrpcRequestDTO
            {
                UserId = userId.ToString(),
            };

            return await client.GetEmailAsync(request);
        }
    }
}
