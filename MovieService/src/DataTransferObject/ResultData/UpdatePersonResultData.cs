using Shared.Contracts.ResultData;

namespace src.DataTransferObject.ResultData
{
    public class UpdatePersonResultData : BaseResultData
    {
        public UpdatePersonDataResult Data { get; set; }
    }

    public class UpdatePersonDataResult : PersonBaseDataResult
    {
    }
}
