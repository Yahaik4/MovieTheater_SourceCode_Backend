using Shared.Contracts.ResultData;

namespace MovieService.DataTransferObject.ResultData
{
    public class UpdatePersonResultData : BaseResultData
    {
        public UpdatePersonDataResult Data { get; set; }
    }

    public class UpdatePersonDataResult : PersonBaseDataResult
    {
    }
}
