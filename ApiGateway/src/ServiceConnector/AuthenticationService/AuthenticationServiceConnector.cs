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

        public async Task<LoginGrpcReplyDTO> Login(string email, string password, string ipAddress, string device)
        {
            using var channel = GetAuthenticationServiceChannel();
            var client = new AuthenticationGrpcService.AuthenticationGrpcServiceClient(channel);

            var headers = new Metadata
            {
                {"ipAddress",  ipAddress},
                {"device", device}
            };
                
            var request = new LoginGrpcRequestDTO
            {
                Email = email,
                Password = password
            };

            return await client.LoginAsync(request, headers);
        }

        public async Task<RefreshTokenGrpcReplyDTO> RefreshToken(string refreshToken)
        {
            using var channel = GetAuthenticationServiceChannel();
            var client = new AuthenticationGrpcService.AuthenticationGrpcServiceClient(channel);

            var request = new RefreshTokenGrpcRequestDTO
            {
                RefreshToken = refreshToken
            };

            return await client.RefreshTokenAsync(request);
        }

        public async Task<LogoutGrpcReplyDTO> Logout(string refreshToken)
        {
            using var channel = GetAuthenticationServiceChannel();
            var client = new AuthenticationGrpcService.AuthenticationGrpcServiceClient(channel);

            var request = new RefreshTokenGrpcRequestDTO
            {
                RefreshToken = refreshToken
            };

            return await client.LogoutAsync(request);
        }

        public async Task<RegisterGrpcReplyDTO> Register(string fullName, string email, string Password)
        {
            using var channel = GetAuthenticationServiceChannel();
            var client = new AuthenticationGrpcService.AuthenticationGrpcServiceClient(channel);

            var request = new RegisterGrpcRequestDTO
            {
                FullName = fullName,
                Password = Password,
                Email = email,
            };

            return await client.RegisterAsync(request);
        }

    }
}
