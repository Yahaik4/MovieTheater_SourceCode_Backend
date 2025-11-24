using ApiGateway.DataTransferObject.Parameter;
using ApiGateway.DataTransferObject.ResultData;
using ApiGateway.ServiceConnector.AuthenticationService;
using ApiGateway.ServiceConnector.OTPService;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Contracts.Constants;
using Shared.Utils;
using Microsoft.AspNetCore.Authorization;
using Shared.Contracts.Enums;
using ApiGateway.Helper;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationServiceConnector _authenticationConnector;
        private readonly OTPServiceConnector _otpServiceConnector;
        private readonly ICurrentUserService _currentUserService;

        public AuthenticationController(AuthenticationServiceConnector authenticationServiceConnector, OTPServiceConnector otpServiceConnector, ICurrentUserService currentUserService)
        {
            _authenticationConnector = authenticationServiceConnector;
            _otpServiceConnector = otpServiceConnector;
            _currentUserService = currentUserService;
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
                    Data = result.Data == null 
                        ? null 
                        : new RegisterDataResult
                        {
                            UserId = Guid.Parse(result.Data.UserId)
                        }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Register Error: {message}");

                return new RegisterResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpPut("update-profile")]
        [Authorize]
        public async Task<UpdateUserResultDTO> UpdateCustomer(
            [FromBody] UpdateUserRequestParam param)
        {
            try
            {
                var userId = _currentUserService.UserId;
                var callerRole = _currentUserService.Role ?? string.Empty;
                var callerPosition = _currentUserService.Position;

                var result = await _authenticationConnector.UpdateCustomer(
                    targetUserId: Guid.Parse(userId),
                    callerRole: callerRole,
                    callerPosition: callerPosition,
                    fullName: param.FullName,
                    phoneNumber: param.PhoneNumber,
                    dayOfBirth: param.DayOfBirth,
                    gender: param.Gender,
                    points: null
                );

                return new UpdateUserResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode
                };
            }
            catch (Grpc.Core.RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                return new UpdateUserResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpPost("verify-account")]
        public async Task<VerifyAccountResultDTO> VerifyOTP(VerifyAccountRequestParam param)
        {
            try
            {
                var result = await _otpServiceConnector.VerifyOTP(param.UserId, param.Code, OtpPurposeConstants.Register);

                if (!result.Result)
                {
                    return new VerifyAccountResultDTO
                    {
                        Result = false,
                        Message = result.Message,
                        StatusCode = result.StatusCode,
                    };
                }

                var verifyAccount = await _authenticationConnector.VerifyAccount(param.UserId);

                return new VerifyAccountResultDTO
                {
                    Result = verifyAccount.Result,
                    Message = verifyAccount.Message,
                    StatusCode = verifyAccount.StatusCode,
                };
                
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new VerifyAccountResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpPost("resend-otp")]
        public async Task<ResendOTPResultDTO> ResendOTP(ResendOTPRequestParam param)
        {
            try
            {
                var result = await _authenticationConnector.ResendOTP(param.email, param.Purpose);

                return new ResendOTPResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data == null ? null : new ResendOTPDataResult
                    {
                        Code = result.Data.Code,
                        Expiry = result.Data.Expiry
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                return new ResendOTPResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpPost("forgot-password")]
        public async Task<ForgotPasswordResultDTO> ForgotPassword([FromBody] ForgotPasswordRequestParam param)
        {
            try
            {
                var result = await _authenticationConnector.ResendOTP(param.Email, OtpPurposeConstants.ResetPassword);

                return new ForgotPasswordResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                return new ForgotPasswordResultDTO { Result = false, Message = message, StatusCode = (int)statusCode };
            }
        }

        [HttpPost("verify-password")]
        public async Task<VerifyPasswordResultDTO> VerifyPassword([FromBody] VerifyPasswordRequestParam param)
        {
            try
            {
                var result = await _authenticationConnector.VerifyAndResetPassword(
                    email: param.Email,
                    otp: param.Otp,
                    newPassword: param.NewPassword
                );

                return new VerifyPasswordResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                return new VerifyPasswordResultDTO { Result = false, Message = message, StatusCode = (int)statusCode };
            }
        }

    }
}
