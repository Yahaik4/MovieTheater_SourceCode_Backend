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
        
        public async Task<RegisterGrpcReplyDTO> AddUser(string fullName, string email, string password, string role, string? phoneNumber, string? dayOfBirth, string? gender, Guid? cinemaId, string? position, string? salary)
        {
            using var channel = GetAuthenticationServiceChannel();
            var client = new AuthenticationGrpcService.AuthenticationGrpcServiceClient(channel);

            var request = new AddUserGrpcRequestDTO
            {
                FullName = fullName,
                Email = email,
                Password = password,
                Role = role,
                PhoneNumber = phoneNumber ?? string.Empty,
                DayOfBirth = dayOfBirth ?? string.Empty,
                Gender = gender ?? string.Empty,
                CinemaId = cinemaId?.ToString() ?? string.Empty,
                Position = position ?? string.Empty,
                Salary = salary ?? string.Empty
            };

            return await client.AddUserAsync(request);
        }

        public async Task<GetCustomersGrpcReplyDTO> GetCustomers(string? userId)
        {
            using var channel = GetAuthenticationServiceChannel();
            var client = new AuthenticationGrpcService.AuthenticationGrpcServiceClient(channel);

            var request = new GetUsersGrpcRequestDTO
            {
                UserId = userId ?? string.Empty   // rỗng = lấy tất cả
            };

            return await client.GetCustomersAsync(request);
        }

        public async Task<GetStaffsGrpcReplyDTO> GetStaffs(string? userId, Guid? cinemaId)
        {
            using var channel = GetAuthenticationServiceChannel();
            var client = new AuthenticationGrpcService.AuthenticationGrpcServiceClient(channel);

            var request = new GetUsersGrpcRequestDTO
            {
                UserId = userId ?? string.Empty,
                CinemaId = cinemaId?.ToString() ?? string.Empty   // NEW
            };

            return await client.GetStaffsAsync(request);
        }

        public async Task<DeleteUserGrpcReplyDTO> DeleteUser(
            Guid targetUserId,
            string callerRole,
            string? callerPosition)
        {
            using var channel = GetAuthenticationServiceChannel();
            var client = new AuthenticationGrpcService.AuthenticationGrpcServiceClient(channel);

            var request = new DeleteUserGrpcRequestDTO
            {
                TargetUserId = targetUserId.ToString(),
                CallerRole = callerRole ?? string.Empty,
                CallerPosition = callerPosition ?? string.Empty
            };

            return await client.DeleteUserAsync(request);
        }

        public async Task<UpdateCustomerGrpcReplyDTO> UpdateCustomer(
            Guid targetUserId,
            string callerRole,
            string? callerPosition,
            string? fullName,
            string? phoneNumber,
            string? dayOfBirth,
            string? gender,
            int? points)
        {
            using var channel = GetAuthenticationServiceChannel();
            var client = new AuthenticationGrpcService.AuthenticationGrpcServiceClient(channel);

            var request = new UpdateCustomerGrpcRequestDTO
            {
                TargetUserId = targetUserId.ToString(),
                CallerRole = callerRole ?? string.Empty,
                CallerPosition = callerPosition ?? string.Empty,
                FullName = fullName ?? string.Empty,
                PhoneNumber = phoneNumber ?? string.Empty,
                DayOfBirth = dayOfBirth ?? string.Empty,
                Gender = gender ?? string.Empty,
                Points = points?.ToString() ?? string.Empty
            };

            return await client.UpdateCustomerAsync(request);
        }

        public async Task<UpdateStaffGrpcReplyDTO> UpdateStaff(
            Guid targetUserId,
            string callerRole,
            string? callerPosition,
            string? fullName,
            string? phoneNumber,
            string? dayOfBirth,
            string? gender,
            Guid? cinemaId,
            string? position,
            decimal? salary)
        {
            using var channel = GetAuthenticationServiceChannel();
            var client = new AuthenticationGrpcService.AuthenticationGrpcServiceClient(channel);

            var request = new UpdateStaffGrpcRequestDTO
            {
                TargetUserId = targetUserId.ToString(),
                CallerRole = callerRole ?? string.Empty,
                CallerPosition = callerPosition ?? string.Empty,
                FullName = fullName ?? string.Empty,
                PhoneNumber = phoneNumber ?? string.Empty,
                DayOfBirth = dayOfBirth ?? string.Empty,
                Gender = gender ?? string.Empty,
                CinemaId = cinemaId?.ToString() ?? string.Empty,
                Position = position ?? string.Empty,
                Salary = salary?.ToString() ?? string.Empty
            };

            return await client.UpdateStaffAsync(request);
        }
    }
}
