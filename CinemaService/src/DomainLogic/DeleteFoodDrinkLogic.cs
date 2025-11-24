using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class DeleteFoodDrinkLogic : IDomainLogic<DeleteFoodDrinkParam, Task<DeleteFoodDrinkResultData>>
    {
        private readonly IFoodDrinkRepository _foodDrinkRepository;

        public DeleteFoodDrinkLogic(IFoodDrinkRepository foodDrinkRepository)
        {
            _foodDrinkRepository = foodDrinkRepository;
        }

        public async Task<DeleteFoodDrinkResultData> Execute(DeleteFoodDrinkParam param)
        {
            var entity = await _foodDrinkRepository.GetByIdAsync(param.Id);

            if (entity == null)
                throw new NotFoundException("Food & drink not found");

            await _foodDrinkRepository.DeleteAsync(entity);

            return new DeleteFoodDrinkResultData
            {
                Result = true,
                Message = "Delete food & drink successfully",
                StatusCode = StatusCodeEnum.Success
            };
        }
    }
}
