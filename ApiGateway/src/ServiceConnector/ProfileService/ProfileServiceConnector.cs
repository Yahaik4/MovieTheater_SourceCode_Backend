using ProfileGrpc;

namespace ApiGateway.ServiceConnector.ProfileService
{
    public class ProfileServiceConnector : BaseServiceConnector
    {
        private readonly ServiceConnectorConfig _serviceConnectorConfig;

        public ProfileServiceConnector(IConfiguration configuration) : base(configuration)
        {
            _serviceConnectorConfig = GetServiceConnectorConfig();
        }

        public async Task<GetProfileGrpcReplyDTO> GetProfile(string userId)
        {
            using var channel = GetProfileServiceChannel();
            var client = new ProfileGrpcService.ProfileGrpcServiceClient(channel);

            var request = new GetProfileGrpcRequestDTO
            {
                UserId = userId,

            };

            return await client.GetProfileAsync(request);
        }
    }
}
