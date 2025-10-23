using Shared.Contracts.ResultData;

namespace src.DataTransferObject.ResultData
{
    public class UpdateCinemaResultData : BaseResultData
    {
        public UpdateCinemaDataResult Data { get; set; }
    }

    public class UpdateCinemaDataResult : CinemaBaseDataResult
    {
    }
}
