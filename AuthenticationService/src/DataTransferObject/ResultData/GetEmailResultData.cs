using Shared.Contracts.ResultData;

namespace AuthenticationService.DataTransferObject.ResultData
{
    public class GetEmailResultData : BaseResultData
    {
        public GetEmailDataResult Data { get; set; }
    }

    public class GetEmailDataResult
    {
        public string Email { get; set; }
    }
}
