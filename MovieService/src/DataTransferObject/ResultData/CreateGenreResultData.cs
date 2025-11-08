using Shared.Contracts.ResultData;

namespace MovieService.DataTransferObject.ResultData
{
    public class CreateGenreResultData : BaseResultData
    {
        public CreateGenreDataResult Data { get; set; }
    }

    public class CreateGenreDataResult : GenreBaseDataResult
    { 
    }
}
