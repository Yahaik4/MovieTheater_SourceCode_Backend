using ApiGateway.DataTransferObject.ResultData;
using ApiGateway.ServiceConnector.ProfileService;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileServiceConnector _profileServiceConnector;

        public ProfileController(ProfileServiceConnector profileServiceConnector)
        {
            _profileServiceConnector = profileServiceConnector;
        }

        [HttpGet("profile")]
        public async Task<GetProfileResultDTO> GetProfile()
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var email = User.FindFirstValue(JwtRegisteredClaimNames.Email);

            try
            {
                var result = await _profileServiceConnector.GetProfile(userId);

                return new GetProfileResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new GetProfileDataResult
                    {
                        FullName = result.Data.FullName,
                        Email = email,
                        Address = result.Data.Address,
                        DayOfBirth = string.IsNullOrEmpty(result.Data.DayOfBirth) ? null : DateOnly.Parse(result.Data.DayOfBirth),   
                        Gender = result.Data.Gender,
                        PhoneNumer = result.Data.PhoneNumber,
                        Points = result.Data.Points,
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Get Profile Error: {message}");

                return new GetProfileResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

    }
}
