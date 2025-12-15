using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class GetPromotionsParam : IParam
    {
        public Guid? Id { get; set; }
        public string? Code { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? DiscountType { get; set; }
        public bool? IsActive { get; set; }
    }
}
