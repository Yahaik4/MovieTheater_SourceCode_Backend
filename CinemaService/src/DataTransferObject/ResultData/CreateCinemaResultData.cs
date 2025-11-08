using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class CreateCinemaResultData : BaseResultData
    {
        public CreateCinemaDataResult Data { get; set; } = null!;
    }

    public class CreateCinemaDataResult : CinemaBaseDataResult
    {
        public string CreateBy { get; set; }
    }
}
