using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class CreateFoodDrinkLogic : IDomainLogic<CreateFoodDrinkParam, Task<CreateFoodDrinkResultData>>
    {
        private readonly IFoodDrinkRepository _foodDrinkRepository;

        public CreateFoodDrinkLogic(IFoodDrinkRepository foodDrinkRepository)
        {
            _foodDrinkRepository = foodDrinkRepository;
        }

        public async Task<CreateFoodDrinkResultData> Execute(CreateFoodDrinkParam param)
        {
            if (string.IsNullOrWhiteSpace(param.Name))
                throw new ValidationException("Name is required");

            if (param.Price <= 0)
                throw new ValidationException("Price must be greater than 0");

            var normalizedImage = NormalizeBase64OrNull(param.Image);

            var entity = new FoodDrink
            {
                Id = Guid.NewGuid(),
                Name = param.Name,
                Type = param.Type,
                Size = param.Size,
                Price = param.Price,
                Image = normalizedImage,
                Description = string.IsNullOrWhiteSpace(param.Description) ? null : param.Description.Trim(),
                CreatedBy = param.CreatedBy ?? "System"
            };

            var created = await _foodDrinkRepository.CreateAsync(entity);

            return new CreateFoodDrinkResultData
            {
                Result = true,
                Message = "Create food & drink successfully",
                StatusCode = StatusCodeEnum.Created,
                Data = new CreateFoodDrinkDataResult
                {
                    Id = created.Id,
                    Name = created.Name,
                    Type = created.Type,
                    Size = created.Size,
                    Price = created.Price,
                    Image = created.Image,
                    Description = created.Description,
                    CreatedBy = created.CreatedBy
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
