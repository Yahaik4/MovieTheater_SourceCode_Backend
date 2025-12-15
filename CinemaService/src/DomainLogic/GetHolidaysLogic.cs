using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class GetHolidaysLogic : IDomainLogic<GetHolidaysParam, Task<GetHolidaysResultData>>
    {
        private readonly IHolidayRepository _holidayRepository;

        public GetHolidaysLogic(IHolidayRepository holidayRepository)
        {
            _holidayRepository = holidayRepository;
        }

        public async Task<GetHolidaysResultData> Execute(GetHolidaysParam param)
        {
            var holidays = await _holidayRepository.GetHolidays(param.Name, param.StartDate, param.EndDate);

            return new GetHolidaysResultData
            {
                Result = true,
                Message = "Get Holiday Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = holidays.Select(h => new GetHolidaysDataResult
                {
                    Id = h.Id,
                    Name = h.Name,
                    Day = h.Day,
                    Month = h.Month,
                    ExtraPrice = h.ExtraPrice,
                }).ToList()
            };
        }
    }
}
