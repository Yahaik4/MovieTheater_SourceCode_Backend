using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Helper;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace src.DomainLogic
{
    public class LoginLogic : IDomainLogic<LoginParam, Task<LoginResultData>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly JwtToken _jwtToken;

        public LoginLogic(IUserRepository userRepository, ISessionRepository sessionRepository ,JwtToken jwtToken)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _jwtToken = jwtToken;
        }

        public async Task<LoginResultData> Execute(LoginParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var user = await _userRepository.GetUserByEmail(param.Email);

            if (user == null)
            {
                return new LoginResultData
                {
                    Result = false,
                    Message = "Email is not registered",
                    StatusCode = Common.StatusCodeEnum.NotFound,
                    Data = null
                };
            }

            //if (!VerifyPassword(param.Password, user.Password))
            //{
            //    return new LoginResultData
            //    {
            //        Result = false,
            //        Message = "Invalid password",
            //        Data = null
            //    };
            //}

            var accessToken = _jwtToken.GenerateAccessToken(user.UserId, user.Email, user.Role);
            var sessionId = Guid.NewGuid();
            
            await _sessionRepository.CreateSession(new Session
            {
                SessionId = sessionId,
                UserId = user.UserId,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IpAddress = param.IpAddress,
                Device = param.Device,
            });

            var refreshToken = _jwtToken.GenerateRefreshToken(sessionId.ToString());

            return new LoginResultData
            {
                Result = true,
                Message = "Login successfully",
                StatusCode = Common.StatusCodeEnum.Success,
                Data = new LoginDataResult
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                }
            };
        }


        private bool VerifyPassword(string password, string hashedPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                var computedHash = Convert.ToBase64String(hash);

                return computedHash == hashedPassword;
            }
        }
    }
}
