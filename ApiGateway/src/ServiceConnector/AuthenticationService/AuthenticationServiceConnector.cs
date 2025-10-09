using AuthenticationGrpc;
using Grpc.Core;

namespace src.ServiceConnector.AuthServiceConnector
{
    public class AuthenticationServiceConnector : BaseServiceConnector
    {
        private readonly ServiceConnectorConfig _serviceConnectorConfig;

        public AuthenticationServiceConnector(IConfiguration configuration) : base(configuration)
        {
            _serviceConnectorConfig = GetServiceConnectorConfig();
        }

        public async Task<LoginGprcReplyDTO> Login(string email, string password, string ipAddress, string device)
        {
            using var channel = GetAuthenticationServiceChannel();
            var client = new AuthenticationGrpcService.AuthenticationGrpcServiceClient(channel);

            var headers = new Metadata
            {
                {"ipAddress",  ipAddress},
                {"device", device}
            };
                
            var request = new LoginGprcRequestDTO
            {
                Email = email,
                Password = password
            };

            return await client.LoginAsync(request, headers);
        }
    }
}
