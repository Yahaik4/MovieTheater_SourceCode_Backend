namespace ApiGateway.DataTransferObject.Parameter
{
    public class UpdatePromotionRequestParam
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? MinOrderValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? LimitPerUser { get; set; }
        public int? LimitTotalUse { get; set; }
        public int? UsedCount { get; set; }
        public bool? IsActive { get; set; }
    }
}
