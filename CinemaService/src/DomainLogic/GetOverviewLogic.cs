//using CinemaService.DataTransferObject.Parameter;
//using CinemaService.DataTransferObject.ResultData;
//using CinemaService.Infrastructure.Repositories.Interfaces;
//using Shared.Contracts.Exceptions;
//using Shared.Contracts.Interfaces;

//namespace CinemaService.DomainLogic
//{
//    public class GetOverviewLogic : IDomainLogic<GetOverviewParam, Task<GetOverviewResultData>>
//    {
//        private readonly IBookingRepository _bookingRepository;

//        public GetOverviewLogic(IBookingRepository bookingRepository)
//        {
//            _bookingRepository = bookingRepository;
//        }

//        public async Task<GetOverviewResultData> Execute(GetOverviewParam param)
//        {
//            //if(param.CinemaId != null)
//            //{

//            //}

//            if(param.RangeType != null)
//            {
//                switch (param.RangeType.ToLower())
//                {
//                    case "day":
//                        totalPrice -= totalPrice * (promotion.DiscountValue / 100m);
//                        break;

//                    case "week":
//                        totalPrice -= promotion.DiscountValue;
//                        break;

//                    case "month":
//                        totalPrice -= promotion.DiscountValue;
//                        break;

//                    case "year":
//                        totalPrice -= promotion.DiscountValue;
//                        break;

//                    default:
//                        throw new ValidationException("Invalid promotion discount type.");
//                }
//            }

//            if (param.RangeType != null) 
//            { 
                
//            }
//        }

//        private async Task<GetOverviewDataResult> GetOverviewByDay()
//        {
//            var booking = await _bookingRepository.GetBookingPaid
//        }
//    }
//}
