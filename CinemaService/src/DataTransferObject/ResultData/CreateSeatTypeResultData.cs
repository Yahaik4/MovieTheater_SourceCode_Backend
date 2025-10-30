using Shared.Contracts.ResultData;

namespace src.DataTransferObject.ResultData
{
    public class CreateSeatTypeResultData : BaseResultData
    {
        public CreateSeatTypeDataResult Data { get; set; } = null!;
    }

    public class CreateSeatTypeDataResult
    {
        public Guid Id { get; set; }
        public string Type { get; set; } // VIP, Couple, Standard.
        public decimal ExtraPrice { get; set; }
    }
}
