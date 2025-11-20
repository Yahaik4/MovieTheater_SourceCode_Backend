using OTPService.DataTransferObject.Parameter;
using OTPService.DataTransferObject.ResultData;
using OTPService.Infrastructure.Repositories.Interfaces;
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
            var otp = await _otpRepository.GetOTPbyUserId(param.UserId);

            if (otp == null)
            {
                return new VerifyOTPResultData
                {
                    Result = false,
                    Message = "OTP not found",
                    StatusCode = StatusCodeEnum.NotFound
                };
            }


            if (otp.Code != param.Code || otp.ExpiryAt < DateTime.UtcNow)
            {
                return new VerifyOTPResultData
                {
                    Result = false,
                    Message = "Invalid OTP code",
                    StatusCode = StatusCodeEnum.BadRequest
                };
            }

            return new VerifyOTPResultData
            {
                Result = true,
                Message = "Verify OTP Successfully",
                StatusCode = StatusCodeEnum.Success
            };
        }
    }
}
