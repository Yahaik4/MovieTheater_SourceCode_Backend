using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Utils;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.ServiceConnector.OtpService;


namespace src.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class OtpController : ControllerBase
    {
        private readonly OtpServiceConnector _otpServiceConnector;

        public OtpController(OtpServiceConnector otpServiceConnector)
        {
            _otpServiceConnector = otpServiceConnector;
        }

        [HttpPost("generate-otp")]
        public async Task<GenerateOtpResult> GenerateOtp([FromBody] GenerateOtpRequest request)
        {
            try
            {
                var result = await _otpServiceConnector.GenerateOtpAsync(request.Email);

                return new GenerateOtpResult
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new GenerateOtpData
                    {
                        Email = result.Data.Email,
                        Otp = result.Data.Otp
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new GenerateOtpResult
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpPost("validate")]
        public async Task<ValidateOtpResult> ValidateOTP([FromBody] ValidateOtpRequestParam request)
        {
            try
            {
                var result = await _otpServiceConnector.ValidateOtpAsync(request.Email, request.Otp);

                return new ValidateOtpResult
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new ValidateOtpData
                    {
                        Email = result.Data.Email,
                        IsValid = result.Data.IsValid
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Validate OTP Error: {message}");

                return new ValidateOtpResult
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        } 
    }
}