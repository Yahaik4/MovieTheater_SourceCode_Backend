using System.ComponentModel.DataAnnotations;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Helper;
using src.Infrastructure.Repositories.Interfaces;
using src.Templates;

namespace src.DomainLogic;

public class SendOtpLogic : IDomainLogic<GenerateOtpRequest, Task<GenerateOtpResult>>
{
    private IOtpRepository _otpRepository;
    private readonly Email _emailService;

    public SendOtpLogic(IOtpRepository otpRepository, Email emailService)
    {
        _otpRepository = otpRepository;
        _emailService = emailService;
    }

    public async Task<GenerateOtpResult> Execute(GenerateOtpRequest request)
    {
        if (string.IsNullOrEmpty(request.Email))
            throw new ValidationException("Email is required.");

        await _otpRepository.DeleteOtpAsync(request.Email);

        var otp = GenerateRandomOtp(6);

        await _otpRepository.SaveOtpAsync(request.Email, otp, TimeSpan.FromMinutes(5));

        string htmlContent = OTPTemplate.Generate(otp);

        var sent = await _emailService.SendOtpAsync(request.Email, "Verification Code", htmlContent);

        if (!sent)
            throw new ValidationException("Failed to send OTP email.");
                
        return new GenerateOtpResult
        {
            Result = true,
            Message = "OTP generated and sent successfully.",
            StatusCode = StatusCodeEnum.Success,
            Data = new GenerateOtpData
            {
                Email = request.Email,
                Otp = otp
            }
        };
    }
    

    private string GenerateRandomOtp(int length)
    {
        const string chars = "0123456789";
        var random = new Random();
        var otp = new char[length];

        for (int i = 0; i < length; i++)
        {
            otp[i] = chars[random.Next(chars.Length)];
        }

        return new string(otp);
    }
}