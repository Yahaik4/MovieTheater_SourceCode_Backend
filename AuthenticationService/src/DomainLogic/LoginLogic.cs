using AuthenticationService.DataTransferObject.Parameter;
using AuthenticationService.DataTransferObject.ResultData;
using AuthenticationService.Helper;
using AuthenticationService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using AuthenticationService.Infrastructure.EF.Models;
using System.Security.Cryptography;
using System.Text;
using AuthenticationService.ServiceConnector.ProfileService;

namespace AuthenticationService.DomainLogic
{
    public class LoginLogic : IDomainLogic<LoginParam, Task<LoginResultData>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly JwtToken _jwtToken;
        private readonly ProfileServiceConnector _profileServiceConnector;

        public LoginLogic(
            IUserRepository userRepository,
            ISessionRepository sessionRepository,
            JwtToken jwtToken,
            ProfileServiceConnector profileServiceConnector)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _jwtToken = jwtToken;
            _profileServiceConnector = profileServiceConnector;
        }

        public async Task<LoginResultData> Execute(LoginParam param)
        {
            if (param == null)
                throw new ValidationException("Param cannot be blank.");

            var user = await _userRepository.GetUserByEmail(param.Email);

            if (user == null)
                throw new NotFoundException("Email is not registered");

            if (user.IsDeleted)
                throw new UnauthorizedException("Account locked.");

            // TODO: bật lại khi hết DEV
            if (!VerifyPassword(param.Password, user.Password))
                return new LoginResultData { Result = false, Message = "Invalid password" };

            string? position = null;
            Guid? cinemaId = null;

            if (user.Role == "staff")
            {
                try
                {
                    var staff = await _profileServiceConnector.GetStaffByUserId(user.Id);

                    if (staff != null)
                    {
                        position = staff.Position;
                        cinemaId = staff.CinemaId;
                    }
                }
                catch
                {
                    // Staff profile không lấy được -> login được nhưng không có phân quyền staff
                    position = null;
                    cinemaId = null;
                }
            }

            var accessToken = _jwtToken.GenerateAccessToken(
                user.Id,
                user.Email,
                user.Role,
                position,
                cinemaId
            );

            var sessionId = Guid.NewGuid();

            await _sessionRepository.CreateSession(new Session
            {
                Id = sessionId,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IpAddress = param.IpAddress,
                Device = param.Device
            });

            var refreshToken = _jwtToken.GenerateRefreshToken(sessionId.ToString());

            return new LoginResultData
            {
                Result = true,
                Message = "Login successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new LoginDataResult
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                }
            };
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password))) == hashedPassword;
        }
    }
}
