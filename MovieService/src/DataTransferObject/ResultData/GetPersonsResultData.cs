using Shared.Contracts.ResultData;

namespace MovieService.DataTransferObject.ResultData
{
    public class GetPersonsResultData : BaseResultData
    {
        public List<GetPersonsDataResult> Data { get; set; }
    }

    public class GetPersonsDataResult : PersonBaseDataResult
    {
    }
}
