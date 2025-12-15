namespace ApiGateway.DataTransferObject.ResultData
{
    public class GetPromotionsResultDTO : BaseResultDTO
    {
        public List<GetPromotionsDataResult> Data { get; set; }
    }

    public class GetPromotionsDataResult
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string DiscountType { get; set; } // Percentage | Amount
        public decimal DiscountValue { get; set; } // 10% | 10k,...
        public decimal? MinOrderValue { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int? LimitPerUser { get; set; }
        public int? LimitTotalUse { get; set; }
        public int UsedCount { get; set; }
        public bool IsActive { get; set; }
    }
}
