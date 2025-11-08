using Grpc.Net.Client;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using src.ServiceConnector;
using OTPGrpc;
using Shared.Contracts.Exceptions;
using Shared.Utils;

namespace src.Infrastructure.Connectors
{
    public class OtpServiceConnector : BaseServiceConnector
    {
        private readonly ServiceConnectorConfig _serviceConnectorConfig;

        public OtpServiceConnector(IConfiguration configuration) : base(configuration)
        {
            _serviceConnectorConfig = GetServiceConnectorConfig();
        }

        public async Task<bool> GenerateOtpAsync(string email)
        {
            try
            {
                using var channel = GetOtpServiceChannel(); 
                var client = new OtpGrpcService.OtpGrpcServiceClient(channel);

                var request = new GenerateOtpRequestGrpc
                {
                    Email = email
                };

                var response = await client.GenerateOTPAsync(request);

                if (!response.Result || response.StatusCode != 200)
                {
                    throw new ValidationException($"OTP service error: {response.Message}");
                }

                return true;
            }
            catch (RpcException ex)
            {
                // Parse chi tiết lỗi RPC nếu có
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                throw new ValidationException($"gRPC error from OTP service ({statusCode}): {message}");
            }
            catch (Exception ex)
            {
                throw new ValidationException($"Unexpected error calling OTP service: {ex.Message}");
            }
        }

        // 🔹 Xác minh OTP từ người dùng
        public async Task<bool> ValidateOtpAsync(string email, string otp)
        {
            try
            {
                using var channel = GetOtpServiceChannel();
                var client = new OtpGrpcService.OtpGrpcServiceClient(channel);

                var request = new ValidateOtpRequestGrpc
                {
                    Email = email,
                    Otp = otp
                };

                var response = await client.ValidateOTPAsync(request);

                if (!response.Result)
                {
                    throw new ValidationException($"OTP validation failed: {response.Message}");
                }

                return response.Data.IsValid;
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                throw new ValidationException($"gRPC error from OTP service ({statusCode}): {message}");
            }
            catch (Exception ex)
            {
                throw new ValidationException($"Unexpected error calling OTP service: {ex.Message}");
            }
        }
    }
}
