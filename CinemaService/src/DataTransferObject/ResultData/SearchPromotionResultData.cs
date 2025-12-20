using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class SearchPromotionResultData : BaseResultData
    {
        public GetPromotionsDataResult Data { get; set; }
    }
}
