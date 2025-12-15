using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class UpdatePromotionLogic : IDomainLogic<UpdatePromotionParam, Task<UpdatePromotionResultData>>
    {
        private readonly IPromotionRepository _promotionRepository;

        public UpdatePromotionLogic(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        public async Task<UpdatePromotionResultData> Execute(UpdatePromotionParam param)
        {
            var promotion = await _promotionRepository.GetPromotionById(param.Id);

            if (promotion == null)
            {
                throw new NotFoundException("Promotion not found");
            }

            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                promotion.Code = param.Code;
            }

            if (!string.IsNullOrWhiteSpace(param.Description))
            {
                promotion.Description = param.Description;
            }

            if (!string.IsNullOrWhiteSpace(param.DiscountType))
            {
                promotion.DiscountType = param.DiscountType;
            }

            if (param.DiscountValue.HasValue)
            {
                promotion.DiscountValue = param.DiscountValue.Value;
            }

            if (param.MinOrderValue.HasValue)
            {
                promotion.MinOrderValue = param.MinOrderValue.Value;
            }

            if (param.StartDate.HasValue)
            {
                promotion.StartDate = param.StartDate.Value;
            }

            if (param.EndDate.HasValue)
            {
                promotion.EndDate = param.EndDate.Value;
            }

            if (param.LimitPerUser.HasValue)
            {
                promotion.LimitPerUser = param.LimitPerUser.Value;
            }

            if (param.LimitTotalUse.HasValue)
            {
                promotion.LimitTotalUse = param.LimitTotalUse.Value;
            }

            if (param.IsActive.HasValue)
            {
                promotion.IsActive = param.IsActive.Value;
            }

            promotion.UpdatedAt = DateTime.UtcNow;

            await _promotionRepository.UpdatePromotion(promotion);


            return new UpdatePromotionResultData
            {
                Result = true,
                Message = "Update Promotion Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new UpdatePromotionDataResult
                {
                    Id = promotion.Id,
                    Code = promotion.Code,
                    StartDate = promotion.StartDate.ToString("MM/dd/yyyy hh:mm:ss tt"),
                    EndDate = promotion.EndDate.ToString("MM/dd/yyyy hh:mm:ss tt"),
                    Description = promotion.Description,
                    DiscountType = promotion.DiscountType,
                    DiscountValue = promotion.DiscountValue,
                    LimitPerUser = promotion.LimitPerUser,
                    LimitTotalUse = promotion.LimitTotalUse,
                    MinOrderValue = promotion.MinOrderValue,
                    UsedCount = promotion.UsedCount,
                    IsActive = promotion.IsActive,
                }
            };
        }
    }
}
