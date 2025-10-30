using Shared.Contracts.ResultData;

namespace src.DataTransferObject.ResultData
{
    public class GetAllSeatTypeResultData : BaseResultData
    {
        public List<GetAllSeatTypeDataResult> Data {  get; set; }
    }

    public class GetAllSeatTypeDataResult
    {
        public Guid Id { get; set; }
        public string Type { get; set; } // VIP, Couple, Standard.
        public decimal ExtraPrice { get; set; }
    }
}
