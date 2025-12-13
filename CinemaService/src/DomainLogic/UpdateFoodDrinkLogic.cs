using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
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
                throw new NotFoundException("FoodDrink");

            if (param.Price.HasValue && param.Price.Value <= 0)
                throw new ValidationException("Price must be greater than 0");

            if (!string.IsNullOrWhiteSpace(param.Name))
                entity.Name = param.Name.Trim();

            if (!string.IsNullOrWhiteSpace(param.Type))
                entity.Type = param.Type.Trim();

            if (!string.IsNullOrWhiteSpace(param.Size))
                entity.Size = param.Size.Trim();

            if (param.Price.HasValue)
                entity.Price = param.Price.Value;

            if (param.Image != null) // allow explicitly set null/empty => clear
                entity.Image = NormalizeBase64OrNull(param.Image);

            if (param.Description != null) // allow explicitly set null/empty => clear
                entity.Description = string.IsNullOrWhiteSpace(param.Description) ? null : param.Description.Trim();

            entity.UpdatedAt = DateTime.UtcNow;

            var updatedEntity = await _foodDrinkRepository.UpdateAsync(entity);

            return new UpdateFoodDrinkResultData
            {
                Result = true,
                Message = "Update food & drink successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new UpdateFoodDrinkDataResult
                {
                    Id = updatedEntity.Id,
                    Name = updatedEntity.Name,
                    Type = updatedEntity.Type,
                    Size = updatedEntity.Size,
                    Price = updatedEntity.Price,
                    Image = updatedEntity.Image,
                    Description = updatedEntity.Description
                }
            };
        }

        private static string? NormalizeBase64OrNull(string? image)
        {
            if (string.IsNullOrWhiteSpace(image))
                return null;

            var s = image.Trim();

            var commaIndex = s.IndexOf(',');
            if (s.StartsWith("data:", StringComparison.OrdinalIgnoreCase) && commaIndex >= 0)
                s = s[(commaIndex + 1)..].Trim();

            try
            {
                var bytes = Convert.FromBase64String(s);
                return Convert.ToBase64String(bytes);
            }
            catch (FormatException)
            {
                throw new ValidationException("Image must be a valid base64 string");
            }
        }
    }
}
