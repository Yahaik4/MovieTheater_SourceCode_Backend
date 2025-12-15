using Shared.Infrastructure;

namespace CinemaService.Infrastructure.EF.Models
{
    public class Promotion : BaseEntity
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string DiscountType { get; set; } // Percentage | Amount
        public decimal DiscountValue { get; set; } // 10% | 10k,...
        public decimal? MinOrderValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? LimitPerUser { get; set; }
        public int? LimitTotalUse { get; set; }
        public int UsedCount { get; set; }
        public bool IsActive { get; set; }
    }
}
