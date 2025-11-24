using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class UpdateFoodDrinkLogic : IDomainLogic<UpdateFoodDrinkParam, Task<UpdateFoodDrinkResultData>>
    {
        private readonly IFoodDrinkRepository _foodDrinkRepository;

        public UpdateFoodDrinkLogic(IFoodDrinkRepository foodDrinkRepository)
        {
            _foodDrinkRepository = foodDrinkRepository;
        }

        public async Task<UpdateFoodDrinkResultData> Execute(UpdateFoodDrinkParam param)
        {
            var entity = await _foodDrinkRepository.GetByIdAsync(param.Id);

            if (entity == null)
                throw new NotFoundException("Food & drink not found");

            if (!string.IsNullOrWhiteSpace(param.Name))
                entity.Name = param.Name;

            if (!string.IsNullOrWhiteSpace(param.Type))
                entity.Type = param.Type;

            if (!string.IsNullOrWhiteSpace(param.Size))
                entity.Size = param.Size;

            if (param.Price.HasValue)
            {
                if (param.Price <= 0)
                    throw new ValidationException("Price must be greater than 0");

                entity.Price = param.Price.Value;
            }
            
            entity.UpdatedAt = DateTime.UtcNow;
            var updated = await _foodDrinkRepository.UpdateAsync(entity);

            return new UpdateFoodDrinkResultData
            {
                Result = true,
                Message = "Update food & drink successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new UpdateFoodDrinkDataResult
                {
                    Id = updated.Id,
                    Name = updated.Name,
                    Type = updated.Type,
                    Size = updated.Size,
                    Price = updated.Price
                }
            };
        }
    }
}
