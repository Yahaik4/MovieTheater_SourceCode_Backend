

namespace src.DataTransferObject.ResultData
{
    public class GenerateOtpResult : BaseResultDTO
    {
        public GenerateOtpData Data { get; set; } = null;
    }

    public class GenerateOtpData
    {
        public required string Email { get; set; }
        public required string Otp { get; set; }
    }
}