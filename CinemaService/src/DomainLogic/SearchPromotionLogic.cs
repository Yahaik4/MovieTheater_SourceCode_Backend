using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class SearchPromotionLogic : IDomainLogic<SearchPromotionParam, Task<SearchPromotionResultData>>
    {
        private readonly IPromotionRepository _promotionRepository;

        public SearchPromotionLogic(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        public async Task<SearchPromotionResultData> Execute(SearchPromotionParam param)
        {
            var promotion = await _promotionRepository.SearchPromotionByCode(param.Code);

            if (promotion == null) 
            {
                throw new NotFoundException("Promotion not found");
            }

            return new SearchPromotionResultData
            {
                Result = true,
                Message = "Get Promotion Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new GetPromotionsDataResult
                {
                    Id = promotion.Id,
                    Code = promotion.Code,
                    Description = promotion.Description,
                    StartDate = promotion.StartDate.ToString("MM/dd/yyyy hh:mm:ss tt"),
                    EndDate = promotion.EndDate.ToString("MM/dd/yyyy hh:mm:ss tt"),
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
