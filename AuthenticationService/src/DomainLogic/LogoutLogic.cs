using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Helper;
using src.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;

namespace src.DomainLogic
{
    public class LogoutLogic : IDomainLogic<RefreshTokenParam, Task<LogoutResultData>>
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly JwtToken _jwtToken;
        public LogoutLogic(ISessionRepository sessionRepository, JwtToken jwtToken)
        {
            _sessionRepository = sessionRepository;
            _jwtToken = jwtToken;
        }

        public async Task<LogoutResultData> Execute(RefreshTokenParam param)
        {
            if (param == null)
            {
                throw new ValidationException("Param cannot be blank.");
            }

            Console.WriteLine($"[RefreshTokenLogic] sessionId: {param.RefreshToken}");
            var sessionId = _jwtToken.VerifyRefreshToken(param.RefreshToken);

            var session = await _sessionRepository.GetSessionById(Guid.Parse(sessionId));

            if (session == null)
            {
                throw new UnauthorizedException("Invalid refresh token.");
            }

            session.IsRevoked = true;
            session.ExpiresAt = DateTime.UtcNow;

            var updated = await _sessionRepository.UpdateSession(session);

            return new LogoutResultData
            {
                Result = true,
                Message = "Log out Successfully"
            };
        }
    }
}
