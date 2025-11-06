using Shared.Contracts.ResultData;

namespace src.DataTransferObject.ResultData
{
    public class CreatePersonResultData : BaseResultData
    {
        public CreatePersonDataResult Data { get; set; } 
    }

    public class CreatePersonDataResult : PersonBaseDataResult
    {
    }
}
