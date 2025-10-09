using Microsoft.AspNetCore.Mvc;
using Serilog;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.ServiceConnector.AuthServiceConnector;
using System.Threading.Tasks;

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
                    Data = result.Data == null ? null : new LoginDataResult
                    {
                        AccessToken = result.Data.AccessToken,
                        RefreshToken = result.Data.RefreshToken
                    }
                };

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
    }
}
