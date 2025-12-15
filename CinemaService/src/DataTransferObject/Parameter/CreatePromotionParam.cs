using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class CreatePromotionParam : IParam
    {
        public string Code { get; set; } = default!;
        public string Description { get; set; }
        public string DiscountType { get; set; } = default!;
        public decimal DiscountValue { get; set; }
        public decimal? MinOrderValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? LimitPerUser { get; set; }
        public int? LimitTotalUse { get; set; }
        public bool IsActive { get; set; }
    }
}
