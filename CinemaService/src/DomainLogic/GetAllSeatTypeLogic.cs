using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
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
