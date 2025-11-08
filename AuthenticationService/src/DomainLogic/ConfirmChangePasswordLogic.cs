using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.Repositories.Interfaces;
using src.Infrastructure.Connectors;

namespace src.DomainLogic
{
    public class ConfirmChangePasswordLogic : IDomainLogic<ConfirmChangePasswordParam, Task<ChangePasswordResultData>>
    {
        private readonly IUserRepository _userRepository;
        private readonly OtpServiceConnector _otpConnector;

        public ConfirmChangePasswordLogic(IUserRepository userRepository, OtpServiceConnector otpConnector)
        {
            _userRepository = userRepository;
            _otpConnector = otpConnector;
        }

        public async Task<ChangePasswordResultData> Execute(ConfirmChangePasswordParam param)
        {
            var user = await _userRepository.GetUserByEmail(param.Email)
                ?? throw new NotFoundException("User not found.");

            var otpValid = await _otpConnector.ValidateOtpAsync(param.Email, param.Otp);
            if (!otpValid)
                throw new ValidationException("Invalid or expired OTP.");

            var pendingHash = await _userRepository.GetPendingChangeAsync(param.Email)
                ?? throw new ValidationException("No pending password change found or expired.");

            await _userRepository.ChangePassword(param.Email, pendingHash);
            await _userRepository.DeletePendingChangeAsync(param.Email);

            return new ChangePasswordResultData
            {
                Result = true,
                Message = "Password changed successfully.",
                Data = new ChangePasswordDataResult
                {
                    Email = param.Email,
                    IsOtpSent = false,
                    IsOtpValid = true,
                    IsPasswordChanged = true
                }
            };
        }
    }
}
