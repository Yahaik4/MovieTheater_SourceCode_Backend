using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class GetPromotionsLogic : IDomainLogic<GetPromotionsParam, Task<GetPromotionsResultData>>
    {
        private readonly IPromotionRepository _promotionRepository;

        public GetPromotionsLogic(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        public async Task<GetPromotionsResultData> Execute(GetPromotionsParam param)
        {
            var promotion = await _promotionRepository.GetPromotions(param.Id, param.Code, param.StartDate, param.EndDate, param.DiscountType, param.IsActive);

            return new GetPromotionsResultData
            {
                Result = true,
                Message = "Get Promotion Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = promotion.Select(h => new GetPromotionsDataResult
                {
                    Id = h.Id,
                    Code = h.Code,
                    Description = h.Description,
                    StartDate = h.StartDate.ToString("MM/dd/yyyy hh:mm:ss tt"),
                    EndDate = h.EndDate.ToString("MM/dd/yyyy hh:mm:ss tt"),
                    DiscountType = h.DiscountType,
                    DiscountValue = h.DiscountValue,
                    LimitPerUser = h.LimitPerUser,
                    LimitTotalUse = h.LimitTotalUse,
                    MinOrderValue = h.MinOrderValue,
                    UsedCount = h.UsedCount,
                    IsActive = h.IsActive,
                }).ToList()
            };
        }
    }
}
