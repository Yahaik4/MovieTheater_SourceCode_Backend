using Grpc.Core;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.Repositories.Interfaces;
namespace src.DomainLogic;

public class GenerateOtpLogic : IDomainLogic<GenerateOtpRequest, Task<GenerateOtpResult>>
{
    private IOtpRepository _otpRepository;

    public GenerateOtpLogic(IOtpRepository otpRepository)
    {
        _otpRepository = otpRepository;
    }

    public async Task<GenerateOtpResult> Execute(GenerateOtpRequest request)
    {


        if (string.IsNullOrEmpty(request.Email))
        {
            throw new ValidationException("Email cannot be blank");
        }

        await _otpRepository.DeleteOtpAsync(request.Email);

        var otp = GenerateRandomOtp(6);
        await _otpRepository.SaveOtpAsync(request.Email, otp, TimeSpan.FromMinutes(5));


        return new GenerateOtpResult
        {
            Result = true,
            Message = "Generate OTP successfully",
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
