using Shared.Contracts.ResultData;

namespace src.DataTransferObject.ResultData
{
    public class GetGenresResultData : BaseResultData
    {
        public List<GetGenresDataResult> Data { get; set; }
    }

    public class GetGenresDataResult : GenreBaseDataResult
    {
    }
}
