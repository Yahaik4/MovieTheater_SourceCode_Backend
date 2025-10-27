using CinemaGrpc;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Utils;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.ServiceConnector.AuthServiceConnector;
using src.ServiceConnector.CinemaService;
using System.Text.Json;

namespace src.Controllers
{
    [ApiController]
    [Route("api")]
    public class RequestLocationController : ControllerBase
    {
        private readonly AuthenticationServiceConnector _authenticationConnector;
        private readonly CinemaServiceConnector _cinemaServiceConnector;

        public RequestLocationController(AuthenticationServiceConnector authenticationServiceConnector, CinemaServiceConnector cinemaServiceConnector)
        {
            _authenticationConnector = authenticationServiceConnector;
            _cinemaServiceConnector = cinemaServiceConnector;
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
                    StatusCode = (int) statusCode
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

        [HttpGet("get-list-cinema")]
        public async Task<GetAllCinemasResultDTO> GetAllCinema([FromQuery] GetAllCinemaRequestParam query)
        {
            try
            {
                var result = await _cinemaServiceConnector.GetAllCinemas(query.Id, query.Name, query.City, query.Status);

                return new GetAllCinemasResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data.Select(c => new GetAllCinemasDataResult
                    {
                        Name = c.Name,
                        Address = c.Address,
                        City = c.City,
                        Email = c.Email,
                        PhoneNumber = c.PhoneNumber,
                        Open_Time = c.OpenTime,
                        Close_Time = c.CloseTime,
                        TotalRoom = c.TotalRoom,
                        Status = c.Status,
                    }).ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new GetAllCinemasResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpPost("create-cinema")]
        public async Task<CreateCinemaResultDTO> CreateCinema(CreateCinemaRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.CreateCinema(param.Name, param.Address, param.City, param.PhoneNumber, param.Email, param.Open_Time, param.Close_Time, param.Status);

                return new CreateCinemaResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new CreateCinemaDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        Name = result.Data.Name,
                        Address = result.Data.Address,
                        City = result.Data.City,
                        PhoneNumber = result.Data.PhoneNumber,
                        Email = result.Data.Email,
                        Open_Time = TimeOnly.Parse(result.Data.OpenTime),
                        Close_Time = TimeOnly.Parse(result.Data.CloseTime),
                        Status = result.Data.Status
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new CreateCinemaResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpPut("update-cinema/{id}")]
        public async Task<UpdateCinemaResultDTO> UpdateCinema(Guid id, [FromBody] UpdateCinemaRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.UpdateCinema(id, param.Name, param.Address, param.City, param.PhoneNumber, param.Email, param.Open_Time, param.Close_Time, param.Status);

                return new UpdateCinemaResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new UpdateCinemaDataResult
                    {
                        Name = result.Data.Name,
                        Address = result.Data.Address,
                        City = result.Data.City,
                        PhoneNumber = result.Data.PhoneNumber,
                        Email = result.Data.Email,
                        Open_Time = TimeOnly.Parse(result.Data.OpenTime),
                        Close_Time = TimeOnly.Parse(result.Data.CloseTime),
                        Status = result.Data.Status
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new UpdateCinemaResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpDelete("delete-cinema/{id}")]
        public async Task<DeleteCinemaResultDTO> DeleteCinema(Guid id)
        {
            try
            {
                var result = await _cinemaServiceConnector.DeleteCinema(id);
                return new DeleteCinemaResultDTO
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

                return new DeleteCinemaResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }
    }
}
