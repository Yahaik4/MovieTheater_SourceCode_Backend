using System.Security.Cryptography;
using System.Text;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.Repositories.Interfaces;
using src.Infrastructure.Connectors;
using src.Infrastructure.Repositories;

namespace src.DomainLogic
{
    public class RequestChangePasswordLogic : IDomainLogic<RequestChangePasswordParam, Task<ChangePasswordResultData>>
    {
        private readonly IUserRepository _userRepository;
        private readonly OtpServiceConnector _otpService;

        public RequestChangePasswordLogic(IUserRepository userRepository, OtpServiceConnector otpService)
        {
            _userRepository = userRepository;
            _otpService = otpService;
        }

        public async Task<ChangePasswordResultData> Execute(RequestChangePasswordParam param)
        {
            var user = await _userRepository.GetUserByEmail(param.Email)
                ?? throw new NotFoundException("User not found.");

            var oldHash = HashPassword(param.OldPassword);
            if (user.Password != oldHash)
                throw new UnauthorizedException("Old password incorrect.");

            var newHash = HashPassword(param.NewPassword);
            await _userRepository.SavePendingChangeAsync(param.Email, newHash, TimeSpan.FromMinutes(5));

            var otpSent = await _otpService.GenerateOtpAsync(param.Email);
            if (!otpSent)
                throw new ValidationException("Failed to send OTP.");

            return new ChangePasswordResultData
            {
                Result = true,
                Message = "OTP sent to email.",
                Data = new ChangePasswordDataResult
                {
                    Email = param.Email,
                    IsOtpSent = true,
                    IsPasswordChanged = false,
                    IsOtpValid = false
                }
            };
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}
