// AuthenticationService.DomainLogic/ResetPasswordLogic.cs
using AuthenticationService.Infrastructure.Repositories.Interfaces;
using OTPGrpc;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Enums;
using Grpc.Core;
using AuthenticationService.ServiceConnector.ProfileService;
using AuthenticationGrpc;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationService.DomainLogic
{
    public class ResetPasswordLogic
    {
        private readonly IUserRepository _userRepository;
        private readonly OTPServiceConnector _otpServiceConnector;

        public ResetPasswordLogic(IUserRepository userRepository, OTPServiceConnector otpServiceConnector)
        {
            _userRepository = userRepository;
            _otpServiceConnector = otpServiceConnector;
        }

        public async Task<ResetpassWordReply> Execute(string email, string otp, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            {
                throw new ValidationException("Password must greater than 6 characters and not include white space");
            }

            var user = await _userRepository.GetUserByEmail(email);
            if (user == null) throw new NotFoundException("Account not found or email is not registed");
                
            var verifyResponse = await _otpServiceConnector.VerifyOTP(user.Id, otp, OtpPurposeConstants.ResetPassword);

            if (!verifyResponse.Result)
                throw new ValidationException("Invalid OTP code");

            user.Password = HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateUser(user);

            return new ResetpassWordReply{ 
                Result = true, 
                Message = "Password reset successfully", 
                StatusCode = 200
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