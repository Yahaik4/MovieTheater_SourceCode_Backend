using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class CreateSeatTypeParam : IParam
    {
        public string Type { get; set; } // VIP, Couple, Standard.
        public decimal ExtraPrice { get; set; }
        public string CreatedBy { get; set; }
    }
}
