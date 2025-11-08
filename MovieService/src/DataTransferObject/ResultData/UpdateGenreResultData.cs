using Shared.Contracts.ResultData;

namespace MovieService.DataTransferObject.ResultData
{
    public class UpdateGenreResultData : BaseResultData
    {
        public UpdateGenreDataResult Data { get; set; }
    }

    public class UpdateGenreDataResult : GenreBaseDataResult
    {
    }
}
