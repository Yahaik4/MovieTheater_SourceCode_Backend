using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class GetDailyRevenueReportsLogic : IDomainLogic<GetDailyRevenueReportsParam, Task<GetDailyRevenueReportsResultData>>
    {
        private readonly IDailyRevenueReportRepository _dailyRevenueReportRepository;

        public GetDailyRevenueReportsLogic(IDailyRevenueReportRepository dailyRevenueReportRepository)
        {
            _dailyRevenueReportRepository = dailyRevenueReportRepository;
        }

        public async Task<GetDailyRevenueReportsResultData> Execute(GetDailyRevenueReportsParam param)
        {
            var currentData = await _dailyRevenueReportRepository.GetDailyRevenueReports(param.From, param.To, param.CinemaId);

            int rangeDays = (param.To.DayNumber - param.From.DayNumber) + 1;

            var prevTo = param.From.AddDays(-1);
            var prevFrom = prevTo.AddDays(-rangeDays + 1);

            var previousData = await _dailyRevenueReportRepository.GetDailyRevenueReports(prevFrom, prevTo, param.CinemaId);

            var currentSales = currentData.Sum(x => x.Sales);
            var prevSales = previousData.Sum(x => x.Sales);

            var currentTicket = currentData.Sum(x => x.TicketSold);
            var prevTicket = previousData.Sum(x => x.TicketSold);

            var currentProfit = currentData.Sum(x => x.ProjectedProfit);
            var prevProfit = previousData.Sum(x => x.ProjectedProfit);

            return new GetDailyRevenueReportsResultData
            {
                Result = true,
                Message = "Get Daily Revenue Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new GetDailyRevenueReportsDataResult
                {
                    Sales = currentSales,
                    TicketSold = currentTicket,
                    ProjectedProfit = currentProfit,
                    SalesChangePercent = CalcChangePercent(currentSales, prevSales),
                    TicketSoldChangePercent = CalcChangePercent(currentTicket, prevTicket),
                    ProjectedProfitChangePercent = CalcChangePercent(currentProfit, prevProfit)
                }
            };
        }

        private decimal CalcChangePercent(decimal current, decimal previous)
        {
            if(previous == 0)
                return current == 0 ? 0 : 100;

            return Math.Round(((current - previous) / previous) * 100, 2);
        }
    }
}
