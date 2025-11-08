using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class GetAllSeatTypeLogic : IDomainLogic<GetAllSeatTypeParam, Task<GetAllSeatTypeResultData>>
    {
        private readonly ISeatTypeRepository _seatTypeRepository;

        public GetAllSeatTypeLogic(ISeatTypeRepository seatTypeRepository)
        {
            _seatTypeRepository = seatTypeRepository;
        }

        public async Task<GetAllSeatTypeResultData> Execute(GetAllSeatTypeParam param)
        {
            var seatTypes = await _seatTypeRepository.GetAllSeatType(param.Id, param.Type, param.ExtraPrice);

            return new GetAllSeatTypeResultData
            {
                Result = true,
                Message = "Get All seat types Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = seatTypes.Select(st => new GetAllSeatTypeDataResult
                {
                    Id = st.Id,
                    Type = st.Type,
                    ExtraPrice = st.ExtraPrice
                }).ToList()
            };
        }
    }
}
