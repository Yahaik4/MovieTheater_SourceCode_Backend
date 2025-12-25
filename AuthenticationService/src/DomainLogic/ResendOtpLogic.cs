using AuthenticationGrpc;
using AuthenticationService.Infrastructure.Repositories.Interfaces;
using AuthenticationService.ServiceConnector;
using AuthenticationService.ServiceConnector.ProfileService;
using OTPGrpc;
using Shared.Contracts.Constants;
using Shared.Contracts.Exceptions;
using src.Shared.Contracts.Messages;

public class ResendOtpLogic
{
    private readonly OTPServiceConnector _otpServiceConnector;
    private readonly RabbitMqPublisher _rabbitMqPublisher;
    private readonly IUserRepository _userRepository;

    public ResendOtpLogic(
        OTPServiceConnector otpServiceConnector,
        RabbitMqPublisher rabbitMqPublisher,
        IUserRepository userRepository)
    {
        _otpServiceConnector = otpServiceConnector;
        _rabbitMqPublisher = rabbitMqPublisher;
        _userRepository = userRepository;
    }

    public async Task<ResendOTPReply> Execute(string email, string purpose)
    {   
        var user = await _userRepository.GetUserByEmail(email);

        if(purpose.Equals(OtpPurposeConstants.Register))
        {
            user = await _userRepository.GetNotRegisterdUserByEmail(email);
        }
        
        if (user == null || string.IsNullOrWhiteSpace(user.Email))
        {
            return new ResendOTPReply
            {
                Result = false,
                Message = "User not found or email missing",
                StatusCode = 404
            };
        }

        var otpResponse = await _otpServiceConnector.CreateOTP(user.Id, string.IsNullOrWhiteSpace(purpose) ? "register" : purpose.ToLowerInvariant());

        if (!otpResponse.Result)
        {
            return new ResendOTPReply
            {
                Result = false,
                Message = otpResponse.Message,
                StatusCode = otpResponse.StatusCode
            };
        }

        _rabbitMqPublisher.PublishSendOtp(new SendOtpMessage
        {
            Email = user.Email,
            Otp = otpResponse.Data.Code,
            Purpose = purpose ?? "register"
        });

        return new ResendOTPReply
        {
            Result = true,
            Message = "OTP resent successfully",
            StatusCode = 200,
            Data = new ResendOTPReplyData
            {
                Code = otpResponse.Data.Code,
                Expiry = otpResponse.Data.Expiry
            }
        };
    }
}