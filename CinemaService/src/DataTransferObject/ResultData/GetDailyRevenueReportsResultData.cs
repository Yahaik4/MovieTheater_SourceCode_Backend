using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class GetDailyRevenueReportsResultData : BaseResultData
    { 
        public GetDailyRevenueReportsDataResult Data { get; set; }
    }

    public class GetDailyRevenueReportsDataResult
    {
        public decimal Sales { get; set; }
        public int TicketSold { get; set; }
        public decimal ProjectedProfit { get; set; }
        public decimal SalesChangePercent { get; set; }
        public decimal TicketSoldChangePercent { get; set; }
        public decimal ProjectedProfitChangePercent { get; set; }
    }
}
