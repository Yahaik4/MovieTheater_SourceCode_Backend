using AuthenticationGrpc;
using Grpc.Core;
using ProfileGrpc;

namespace src.ServiceConnector.ProfileServiceConnector
{
    public class ProfileServiceConnector : BaseServiceConnector
    {
        private readonly ServiceConnectorConfig _serviceConnectorConfig;

        public ProfileServiceConnector(IConfiguration configuration) : base(configuration)
        {
            _serviceConnectorConfig = GetServiceConnectorConfig();
        }

        public async Task<CreateProfileGrpcReplyDTO> CreateProfile(string fullname, string Role, string userId)
        {
            using var channel = GetProfileServiceChannel();
            var client = new ProfileGrpcService.ProfileGrpcServiceClient(channel);

            var request = new CreateProfileGrpcRequestDTO
            {
                FullName = fullname,
                Role = Role,
                UserId = userId,
            };

            return await client.CreateProfileAsync(request);
        }
        
    }
}
