using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Utils;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.ServiceConnector.AuthServiceConnector;

namespace src.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationServiceConnector _authenticationConnector;

        public AuthenticationController(AuthenticationServiceConnector authenticationServiceConnector)
        {
            _authenticationConnector = authenticationServiceConnector;
        }

        [HttpPost("sign-in")]
        public async Task<LoginResultDTO> Login(LoginRequestParam param)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            var device = HttpContext.Request.Headers["User-Agent"].ToString();

            try
            {
                var result = await _authenticationConnector.Login(param.Email, param.Password, ipAddress, device);

                var dataReturn = new LoginResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data == null ? null : new LoginDataResult
                    {
                        AccessToken = result.Data.AccessToken,
                        RefreshToken = result.Data.RefreshToken
                    },
                };

                if (result.Result)
                {
                    var httpContext = HttpContext;

                    //httpContext.Response.Cookies.Append("access_token", result.Data.AccessToken, new CookieOptions
                    //{
                    //    HttpOnly = true,
                    //    Secure = true,
                    //    SameSite = SameSiteMode.Strict,
                    //    Expires = DateTimeOffset.UtcNow.AddMinutes(15)
                    //});

                    httpContext.Response.Cookies.Append("refresh_token", result.Data.RefreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(7)
                    });
                }

                return dataReturn;
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new LoginResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpPost("refresh-token")]
        public async Task<RefreshTokenResultDTO> RefreshToken()
        {
            try
            {
                var refreshToken = HttpContext.Request.Cookies["refresh_token"];

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return new RefreshTokenResultDTO
                    {
                        Result = false,
                        Message = "Missing refresh token"
                    };
                }

                var result = await _authenticationConnector.RefreshToken(refreshToken);

                var dataResult = new RefreshTokenResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new RefreshTokenDataResult
                    {
                        AccessToken = result.Data.AccessToken,
                    }
                };

                return dataResult;

            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new RefreshTokenResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpPost("sign-out")]
        public async Task<LogoutResultDTO> SignOut()
        {
            try
            {
                var refreshToken = HttpContext.Request.Cookies["refresh_token"];
                Console.WriteLine($"[RefreshTokenLogic] sessionId: {refreshToken}");

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return new LogoutResultDTO
                    {
                        Result = false,
                        Message = "Missing refresh token",
                    };
                }

                var result = await _authenticationConnector.Logout(refreshToken);

                var dataResult = new LogoutResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                };

                if (result.Result)
                {
                    HttpContext.Response.Cookies.Delete("refresh_token");
                }

                return dataResult;

            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new LogoutResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpPost("register")]
        public async Task<RegisterResultDTO> Register(RegisterRequestParam param)
        {
            try
            {
                var result = await _authenticationConnector.Register(param.FullName, param.Email, param.Password);

                return new RegisterResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new RegisterResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpPost("request_change_password")]
        public async Task<ChangePasswordResultDTO> RequestChangePassword([FromBody] ChangePasswordRequestParam param)
        {
            try
            {
                var result = await _authenticationConnector.RequestChangePassword(
                    param.Email, param.OldPassword, param.NewPassword);

                return new ChangePasswordResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new ChangePasswordDataResult
                    {
                        Email = result.Data.Email,
                        IsOtpSent = true,
                        IsPasswordChanged = false,
                        IsOtpValid = false
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"RequestChangePassword Error: {message}");

                return new ChangePasswordResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpPost("confirm_change_password")]
        public async Task<BaseResultDTO> ConfirmChangePassword([FromBody] ConfirmChangePasswordRequestParam param)
        {
            try
            {
                var result = await _authenticationConnector.ConfirmChangePassword(param.Email, param.Otp);

                return new ChangePasswordResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new ChangePasswordDataResult
                    {
                    Email = result.Data.Email,
                    IsOtpSent = false,
                    IsOtpValid = true,
                    IsPasswordChanged = true
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"ConfirmChangePassword Error: {message}");

                return new ChangePasswordResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

    }
}
