using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class CreatePromotionLogic : IDomainLogic<CreatePromotionParam, Task<CreatePromotionResultData>>
    {
        private readonly IPromotionRepository _promotionRepository;

        public CreatePromotionLogic(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        public async Task<CreatePromotionResultData> Execute(CreatePromotionParam param)
        {
            var promotion = await _promotionRepository.GetPromotionByCode(param.Code);

            if (promotion != null) 
            {
                throw new ConflictException("Promotion Code is existed");
            }

            var newPromotion = new Promotion
            {
                Id = Guid.NewGuid(),
                Code = param.Code,
                Description = param.Description,
                DiscountType = param.DiscountType,
                DiscountValue = param.DiscountValue,
                StartDate = param.StartDate,
                EndDate = param.EndDate,
                LimitPerUser = param.LimitPerUser,
                MinOrderValue = param.MinOrderValue,
                LimitTotalUse = param.LimitTotalUse,
                UsedCount = 0,
                IsActive = param.IsActive,
            };

            await _promotionRepository.CreatePromotion(newPromotion);

            return new CreatePromotionResultData
            {
                Result = true,
                Message = "Create Promotion Successfully",
                StatusCode = StatusCodeEnum.Created,
                Data = new CreatePromotionDataResult
                {
                    Id = newPromotion.Id,
                    Code = newPromotion.Code,
                    Description = newPromotion.Description,
                    DiscountType = newPromotion.DiscountType,
                    DiscountValue = newPromotion.DiscountValue,
                    StartDate = newPromotion.StartDate.ToString("MM/dd/yyyy hh:mm:ss tt"),
                    EndDate = newPromotion.EndDate.ToString("MM/dd/yyyy hh:mm:ss tt"),
                    LimitPerUser = newPromotion.LimitPerUser,
                    LimitTotalUse = newPromotion.LimitTotalUse,
                    MinOrderValue = newPromotion.MinOrderValue,
                    UsedCount= newPromotion.UsedCount,
                    IsActive = newPromotion.IsActive,
                }
            };
        }
    }
}
