using OTPService.DataTransferObject.Parameter;
using OTPService.DataTransferObject.ResultData;
using OTPService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace OTPService.DomainLogic
{
    public class VerifyOTPLogic : IDomainLogic<VerifyOTPParam, Task<VerifyOTPResultData>>
    {
        private readonly IOTPRepository _otpRepository;

        public VerifyOTPLogic(IOTPRepository otpRepository)
        {
            _otpRepository = otpRepository;
        }

        public async Task<VerifyOTPResultData> Execute(VerifyOTPParam param)
        {
            var otp = await _otpRepository.GetOTPByUserAsync(param.UserId, param.Purpose ?? OtpPurposeConstants.Register);

            if (otp == null)
            {
                throw new NotFoundException("OTP not found");
            }


            if (otp.Code != param.Code || otp.ExpiryAt < DateTime.UtcNow)
            {
                throw new ValidationException("Invalid OTP code");
            }

            await _otpRepository.MarkOtpAsDeletedAsync(otp);

            return new VerifyOTPResultData
            {
                Result = true,
                Message = "Verify OTP Successfully",
                StatusCode = StatusCodeEnum.Success
            };
        }
    }
}
