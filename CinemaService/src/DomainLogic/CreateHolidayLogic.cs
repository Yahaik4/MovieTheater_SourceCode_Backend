using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class CreateHolidayLogic : IDomainLogic<CreateHolidayParam, Task<CreateHolidayResultData>>
    {
        private readonly IHolidayRepository _holidayRepository;

        public CreateHolidayLogic(IHolidayRepository holidayRepository)
        {
            _holidayRepository = holidayRepository;
        }

        public async Task<CreateHolidayResultData> Execute(CreateHolidayParam param)
        {
            var holiday = await _holidayRepository.GetHolidayByDate(param.Day, param.Month);

            if(holiday != null)
            {
                throw new ConflictException("Holiday is existed");
            }

            var newHoliday = new Holiday 
            {
                Id = Guid.NewGuid(),
                Name = param.Name,
                Day = param.Day,
                Month = param.Month,
                ExtraPrice = param.ExtraPrice
            };

            await _holidayRepository.CreateHoliday(newHoliday);

            return new CreateHolidayResultData
            {
                Result = true,
                Message = "Create Holiday Successfully",
                StatusCode = StatusCodeEnum.Created,
                Data = new CreateHolidayDataResult
                {
                    Id = newHoliday.Id,
                    Name = newHoliday.Name,
                    Day = newHoliday.Day,
                    Month = newHoliday.Month,
                    ExtraPrice = newHoliday.ExtraPrice
                }
            };
        }
    }
}
