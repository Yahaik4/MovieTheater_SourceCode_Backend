using OTPService.Infrastructure.EF.Models;
using Shared.Contracts.ResultData;

namespace OTPService.DataTransferObject.ResultData
{
    public class CreateOTPResultData : BaseResultData
    {
        public CreateOTPDataResult Data { get; set; }
    }

    public class CreateOTPDataResult {
        public string Code {  get; set; }
        public TimeSpan Expiry {  get; set; }
    }
}
