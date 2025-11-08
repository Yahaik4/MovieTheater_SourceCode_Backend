using Shared.Contracts.ResultData;

namespace src.DataTransferObject.ResultData
{
    public class ChangePasswordResultData : BaseResultData
    {
        public ChangePasswordDataResult Data { get; set; } = null!;
    }

    public class ChangePasswordDataResult
    {
        public string Email { get; set; }
        public bool IsOtpSent { get; set; }
        public bool IsOtpValid { get; set; }
        public bool IsPasswordChanged { get; set; }
    }
}
