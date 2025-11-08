using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class UpdateSeatTypeResultData : BaseResultData
    {
        public UpdateSeatTypeDataResult Data { get; set; }
    }

    public class UpdateSeatTypeDataResult
    {
        public string Type { get; set; } // VIP, Couple, Standard.
        public decimal ExtraPrice { get; set; }
    }
}
