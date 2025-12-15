using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class UpdateHolidayLogic : IDomainLogic<UpdateHolidayParam, Task<UpdateHolidayResultData>>
    {
        private readonly IHolidayRepository _holidayRepository;

        public UpdateHolidayLogic(IHolidayRepository holidayRepository)
        {
            _holidayRepository = holidayRepository;
        }

        public async Task<UpdateHolidayResultData> Execute(UpdateHolidayParam param)
        {
            var holiday = await _holidayRepository.GetHolidaysById(param.Id);

            if (holiday == null) {
                throw new NotFoundException("Holiday not found");
            }

            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                holiday.Name = param.Name;
            }

            if (param.Day.HasValue)
            {
                holiday.Day = param.Day.Value;
            }

            if (param.Month.HasValue)
            {
                holiday.Month = param.Month.Value;
            }

            if (param.ExtraPrice.HasValue)
            {
                holiday.ExtraPrice = param.ExtraPrice.Value;
            }

            holiday.UpdatedAt = DateTime.UtcNow;

            await _holidayRepository.UpdateHoliday(holiday);

            return new UpdateHolidayResultData
            {
                Result = true,
                Message = "Update Holiday Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new UpdateHolidayDataResult
                {
                    Id = holiday.Id,
                    Name = holiday.Name,
                    Day = holiday.Day,
                    Month = holiday.Month,
                    ExtraPrice = holiday.ExtraPrice,
                }
            };
        }
    }
}
