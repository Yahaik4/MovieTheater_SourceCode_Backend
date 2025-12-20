using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class SearchPromotionParam : IParam
    {
        public string Code { get; set; }
    }
}
