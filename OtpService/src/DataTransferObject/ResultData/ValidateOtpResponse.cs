using Shared.Contracts.ResultData;

namespace src.DataTransferObject.ResultData
{
    public class ValidateOtpResult: BaseResultData
    {
        public ValidateOtpData Data { get; set; } = null;
    }

    public class ValidateOtpData
    {
        public required string Email { get; set; }
        public required bool IsValid { get; set; }
    }
}