using AuthenticationService.DataTransferObject.Parameter;
using AuthenticationService.DataTransferObject.ResultData;
using AuthenticationService.Helper;
using AuthenticationService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace AuthenticationService.DomainLogic
{
    public class RefreshTokenLogic : IDomainLogic<RefreshTokenParam, Task<RefreshTokenResultData>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly JwtToken _jwtToken;

        public RefreshTokenLogic(IUserRepository userRepository, ISessionRepository sessionRepository, JwtToken jwtToken) { 
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _jwtToken = jwtToken;
        }

        public async Task<RefreshTokenResultData> Execute(RefreshTokenParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            Console.WriteLine($"[RefreshTokenLogic] sessionId: {param.RefreshToken}");
            var sessionId = _jwtToken.VerifyRefreshToken(param.RefreshToken);

            var session = await _sessionRepository.GetSessionById(Guid.Parse(sessionId));

            if (session == null) 
            {
                throw new UnauthorizedException("Invalid refresh token.");
            }

            if (session.IsRevoked || session.ExpiresAt <= DateTime.UtcNow) {
                throw new UnauthorizedException("Invalid refresh token.");
            }

            var user = await _userRepository.GetUserById(session.UserId);

            return new RefreshTokenResultData
            {
                Result = true,
                Message = "Refresh Token Successfully",
                Data = new RefreshTokenDataResult
                {
                    AccessToken = _jwtToken.GenerateAccessToken(user.Id, user.Email, user.Role)
                }
            };

        }
    }
}
