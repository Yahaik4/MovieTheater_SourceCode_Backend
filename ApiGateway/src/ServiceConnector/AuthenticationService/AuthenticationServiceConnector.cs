using ApiGateway.ServiceConnector;
using AuthenticationGrpc;
using Grpc.Core;
using OTPGrpc;

namespace ApiGateway.ServiceConnector.AuthenticationService
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

        public async Task<VerifyAccountGrpcReplyDTO> VerifyAccount(Guid userId)
        {
            using var channel = GetAuthenticationServiceChannel();
            var client = new AuthenticationGrpcService.AuthenticationGrpcServiceClient(channel);

            var request = new VerifyAccountGrpcRequestDTO
            {
                UserId = userId.ToString(),
            };

            return await client.VerifyAccountAsync(request);
        }

        public async Task<ResendOTPReply> ResendOTP(string email, string? purpose = null)
        {
            using var channel = GetAuthenticationServiceChannel();
            var client = new AuthenticationGrpcService.AuthenticationGrpcServiceClient(channel);

            var request = new ResendOTPRequest
            {
                Email = email,
                Purpose = purpose ?? string.Empty
            };

            return await client.ResendOTPAsync(request);
        }

        public async Task<ResetpassWordReply> VerifyAndResetPassword(string email, string otp, string newPassword)
        {
            using var channel = GetAuthenticationServiceChannel();
            var client = new AuthenticationGrpcService.AuthenticationGrpcServiceClient(channel);

            var request = new ResetPasswordRequest
            {
                Email = email,
                Otp = otp,
                NewPassword = newPassword
            };

            return await client.VerifyResetPasswordAsync(request);
        }
    }
}
