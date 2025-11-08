using Shared.Contracts.ResultData;

namespace src.DataTransferObject.ResultData
{
    public class GenerateOtpResult : BaseResultData
    {
        public GenerateOtpData Data { get; set; } = null;
    }

    public class GenerateOtpData
    {
        public required string Email { get; set; }
        public required string Otp { get; set; }
    }
}