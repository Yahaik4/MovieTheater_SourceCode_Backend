using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Helper;
using src.Infrastructure.Repositories;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
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

            if (session == null) {
                return new RefreshTokenResultData
                {
                    Result = false,
                    Message = "Invalid refresh token",
                    Data = null
                };
            }

            if (session.IsRevoked || session.ExpiresAt <= DateTime.UtcNow) {
                return new RefreshTokenResultData
                {
                    Result = false,
                    Message = "Invalid refresh token",
                    Data = null
                };
            }

            var user = await _userRepository.GetUserById(session.UserId);

            return new RefreshTokenResultData
            {
                Result = true,
                Message = "Refresh Token Successfully",
                Data = new RefreshTokenDataResult
                {
                    AccessToken = _jwtToken.GenerateAccessToken(user.UserId, user.Email, user.Role)
                }
            };

        }
    }
}
