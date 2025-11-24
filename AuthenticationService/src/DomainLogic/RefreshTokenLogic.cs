using AuthenticationService.DataTransferObject.Parameter;
using AuthenticationService.DataTransferObject.ResultData;
using AuthenticationService.Helper;
using AuthenticationService.Infrastructure.Repositories.Interfaces;
using AuthenticationService.ServiceConnector.ProfileService; // NEW
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using Shared.Contracts.Enums;

namespace AuthenticationService.DomainLogic
{
    public class RefreshTokenLogic : IDomainLogic<RefreshTokenParam, Task<RefreshTokenResultData>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly JwtToken _jwtToken;
        private readonly ProfileServiceConnector _profileServiceConnector; // NEW

        public RefreshTokenLogic(
            IUserRepository userRepository,
            ISessionRepository sessionRepository,
            JwtToken jwtToken,
            ProfileServiceConnector profileServiceConnector) // NEW
        { 
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _jwtToken = jwtToken;
            _profileServiceConnector = profileServiceConnector; // NEW
        }

        public async Task<RefreshTokenResultData> Execute(RefreshTokenParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            Console.WriteLine($"[RefreshTokenLogic] refreshToken: {param.RefreshToken}");

            // 1. Verify refresh token
            var sessionIdString = _jwtToken.VerifyRefreshToken(param.RefreshToken);
            if (sessionIdString == null)
            {
                throw new UnauthorizedException("Invalid refresh token.");
            }

            if (!Guid.TryParse(sessionIdString, out var sessionId))
            {
                throw new UnauthorizedException("Invalid refresh token.");
            }

            // 2. Lấy session
            var session = await _sessionRepository.GetSessionById(sessionId);

            if (session == null)
            {
                throw new UnauthorizedException("Invalid refresh token.");
            }

            if (session.IsRevoked || session.ExpiresAt <= DateTime.UtcNow)
            {
                throw new UnauthorizedException("Invalid refresh token.");
            }

            // 3. Lấy user
            var user = await _userRepository.GetUserById(session.UserId);
            if (user == null)
            {
                throw new UnauthorizedException("User not found.");
            }

            // 4. Nếu là staff thì lấy lại position + cinemaId từ ProfileService
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
                    position = null;
                    cinemaId = null;
                }
            }

            var newAccessToken = _jwtToken.GenerateAccessToken(
                user.Id,
                user.Email,
                user.Role,
                position,
                cinemaId
            );

            return new RefreshTokenResultData
            {
                Result = true,
                Message = "Refresh Token Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new RefreshTokenDataResult
                {
                    AccessToken = newAccessToken
                }
            };
        }
    }
}
