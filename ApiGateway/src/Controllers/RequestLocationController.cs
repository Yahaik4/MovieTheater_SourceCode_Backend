using Microsoft.AspNetCore.Mvc;
using Serilog;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.ServiceConnector.AuthServiceConnector;

namespace src.Controllers
{
    [ApiController]
    [Route("api")]
    public class RequestLocationController : ControllerBase
    {
        private readonly AuthenticationServiceConnector _authenticationConnector;

        public RequestLocationController(AuthenticationServiceConnector authenticationServiceConnector)
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
            catch (Exception ex)
            {
                Log.Error($"Login Error: {ex.ToString()}");
                return new LoginResultDTO
                {
                    Result = false,
                    Message = ex.Message,
                };
            }
        }

        [HttpPost("refresh-token")]
        public async Task<RefreshTokenResultDTO> RefreshToken()
        {
            try
            {
                var refreshToken = HttpContext.Request.Cookies["refresh_token"];
                Console.WriteLine($"[RefreshTokenLogic] sessionId: {refreshToken}");

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
            catch (Exception ex)
            {
                Log.Error($"Login Error: {ex.ToString()}");
                return new RefreshTokenResultDTO
                {
                    Result = false,
                    Message = ex.Message,
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
            catch (Exception ex)
            {
                Log.Error($"Login Error: {ex.ToString()}");
                return new LogoutResultDTO
                {
                    Result = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
