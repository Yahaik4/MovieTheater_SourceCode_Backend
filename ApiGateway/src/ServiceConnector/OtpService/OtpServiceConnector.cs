using OTPGrpc;

namespace src.ServiceConnector.OtpService;

public class OtpServiceConnector : BaseServiceConnector
{
    private readonly ServiceConnectorConfig _serviceConnectorConfig;

    public OtpServiceConnector(IConfiguration configuration) : base(configuration)
    {
        _serviceConnectorConfig = GetServiceConnectorConfig();
    }

    private OtpGrpcService.OtpGrpcServiceClient GetOtpGrpcClient()
    {
        var channel = GetOtpServiceChannel();
        return new OtpGrpcService.OtpGrpcServiceClient(channel);
    }

    public async Task<GenerateOtpResultGrpc> GenerateOtpAsync(string email)
    {
        var client = GetOtpGrpcClient();
        var request = new GenerateOtpRequestGrpc { Email = email };

        var response = await client.GenerateOTPAsync(request);
        return response;
    }

    public async Task<ValidateOtpResultGrpc> ValidateOtpAsync(string email, string otp)
    {
        var client = GetOtpGrpcClient();
        var request = new ValidateOtpRequestGrpc
        {
            Email = email,
            Otp = otp
        };

        var response = await client.ValidateOTPAsync(request);
        return response;
    }
}