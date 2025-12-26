using OTPService.DataTransferObject.Parameter;
using OTPService.DataTransferObject.ResultData;
using OTPService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace OTPService.DomainLogic
{
    public class CreateOTPLogic : IDomainLogic<CreateOTPParam, Task<CreateOTPResultData>>
    {
        private readonly IOTPRepository _otpRepository;

        public CreateOTPLogic(IOTPRepository otpRepository)
        {
            _otpRepository = otpRepository;
        }

        public async Task<CreateOTPResultData> Execute(CreateOTPParam param)
        {
            if (param.UserId == Guid.Empty)
            {
                throw new ValidationException("UserId cannot be empty GUID");
            }

            var code = GenerateRandomOtp(6);
            TimeSpan expiry = TimeSpan.FromMinutes(5);
            var otp = await _otpRepository.CreateOrUpdateOTP(param.UserId, code, expiry, param.Purpose ?? OtpPurposeConstants.Register);

            return new CreateOTPResultData
            {
                Result = true,
                Message = "Create OTP Successfully",
                StatusCode = StatusCodeEnum.Created,
                Data = new CreateOTPDataResult
                {
                    Code = otp.Code,
                    Expiry = expiry,
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
}
