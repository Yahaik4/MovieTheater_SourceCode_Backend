using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class GetOverviewResultData : BaseResultData
    {
        public GetOverviewDataResult Data { get; set; }
    }

    public class GetOverviewDataResult
    {
        public PeriodDto Period { get; set; }
        public PeriodDto ComparePeriod { get; set; }
        public KpisDto Kpis { get; set; }
        public List<TicketSalesChartDto> TicketSalesChart { get; set; }
        public List<LocationSalesDto> Locations { get; set; }
    }

    public class PeriodDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class KpisDto
    {
        public KpiValueDto Sales { get; set; }
        public KpiValueDto TicketSold { get; set; }
        public KpiValueDto ProjectedSales { get; set; }
        public KpiValueDto CommissionAds { get; set; }
    }

    public class KpiValueDto
    {
        public decimal Value { get; set; }
        public decimal GrowthPercent { get; set; }
    }

    public class TicketSalesChartDto
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }

    public class LocationSalesDto
    {
        public string Name { get; set; }
        public int Sales { get; set; }
    }
}
