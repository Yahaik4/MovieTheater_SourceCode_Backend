using Grpc.Core;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic;

public class ValidateOtpLogic : IDomainLogic<ValidateOtpRequest, Task<ValidateOtpResult>>
{
    private IOtpRepository _otpRepository;

    public ValidateOtpLogic(IOtpRepository otpRepository)
    {
        _otpRepository = otpRepository;
    }

    public async Task<ValidateOtpResult> Execute(ValidateOtpRequest request)
    {
        var respone = new ValidateOtpResult();

        var storedOtp = _otpRepository.GetOtpAsync(request.Email).Result;

        if (string.IsNullOrEmpty(storedOtp))
            throw new NotFoundException("OTP expired or not exists.");

        if (!string.Equals(storedOtp, request.Otp, StringComparison.OrdinalIgnoreCase))
            throw new ValidationException("Invalid OTP code");

        await _otpRepository.DeleteOtpAsync(request.Email);

        respone.Data = new ValidateOtpData
        {
            Email = request.Email,
            IsValid = true
        };

        respone.StatusCode = StatusCodeEnum.Success;
        respone.Result = true;
        respone.Message = "Verify OTP success";

        return respone;
    }

}