using AuthenticationService.DataTransferObject.Parameter;
using AuthenticationService.DataTransferObject.ResultData;
using AuthenticationService.Infrastructure.EF.Models;
using AuthenticationService.Infrastructure.Repositories.Interfaces;
using AuthenticationService.ServiceConnector;
using AuthenticationService.ServiceConnector.ProfileService;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Messages;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationService.DomainLogic
{
    public class RegisterLogic : IDomainLogic<RegisterParam, Task<RegisterResultData>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ProfileServiceConnector _profileServiceConnector;
        private readonly OTPServiceConnector _otpServiceConnector;
        private readonly RabbitMqPublisher _rabbitMqPublisher;
        public RegisterLogic(IUserRepository userRepository, ProfileServiceConnector profileServiceConnector, OTPServiceConnector oTPServiceConnector, RabbitMqPublisher rabbitMqPublisher)
        {
            _userRepository = userRepository;
            _profileServiceConnector = profileServiceConnector;
            _otpServiceConnector = oTPServiceConnector;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        public async Task<RegisterResultData> Execute(RegisterParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var existed = await _userRepository.GetUserByEmailIncludingUnverified(param.Email);

            if (existed != null)
            {
                if (existed.IsVerified)
                {
                    throw new ValidationException("Email is registerted");
                }

                existed.IsDeleted = true;
                existed.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateUser(existed);
            }

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = param.Email,
                Password = HashPassword(param.Password)
            };

            await _userRepository.CreateUser(newUser);

            try
            {
                var otp = await _otpServiceConnector.CreateOTP(newUser.Id, OtpPurposeConstants.Register);

                if (!otp.Result)
                {
                    await _userRepository.RemoveUser(newUser);
                    throw new UnauthorizedException("Error server while creating OTP");
                }

                var otpMessage = new SendOtpMessage
                {
                    Email = newUser.Email,
                    Otp = otp.Data.Code,
                    Purpose = OtpPurposeConstants.Register
                };
                _rabbitMqPublisher.PublishSendOtp(otpMessage);

                var profile = await _profileServiceConnector.CreateProfile(param.FullName, newUser.Role, newUser.Id.ToString());

                if (!profile.Result)
                {
                    await _userRepository.RemoveUser(newUser);
                    throw new Exception("Error creating profile");
                }
            }
            catch (Exception ex)
            {
                await _userRepository.RemoveUser(newUser);
                return new RegisterResultData
                {
                    Result = false,
                    Message = ex.Message,
                    StatusCode = StatusCodeEnum.InternalServerError,
                };
            }


            return new RegisterResultData
            {
                Result = true,
                Message = "Register Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new RegisterDataResult
                {
                    UserId = newUser.Id,
                }
            };
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                var hashedPassword = Convert.ToBase64String(hash);

                return hashedPassword;
            }
        }

    }
}
