using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class GetAllSeatTypeParam : IParam
    {
        public Guid? Id { get; set; }
        public string? Type { get; set; } // VIP, Couple, Standard.
        public decimal? ExtraPrice { get; set; }
    }
}
