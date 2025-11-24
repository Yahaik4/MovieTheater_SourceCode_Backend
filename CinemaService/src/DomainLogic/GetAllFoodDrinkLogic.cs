using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class GetAllFoodDrinkLogic : IDomainLogic<GetAllFoodDrinkParam, Task<GetAllFoodDrinkResultData>>
    {
        private readonly IFoodDrinkRepository _foodDrinkRepository;

        public GetAllFoodDrinkLogic(IFoodDrinkRepository foodDrinkRepository)
        {
            _foodDrinkRepository = foodDrinkRepository;
        }

        public async Task<GetAllFoodDrinkResultData> Execute(GetAllFoodDrinkParam param)
        {
            var items = await _foodDrinkRepository.GetAllAsync(param.Id, param.Name, param.Type, param.Size);

            return new GetAllFoodDrinkResultData
            {
                Result = true,
                Message = "Get food & drinks successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = items.Select(x => new GetAllFoodDrinkDataResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    Type = x.Type,
                    Size = x.Size,
                    Price = x.Price
                }).ToList()
            };
        }
    }
}
