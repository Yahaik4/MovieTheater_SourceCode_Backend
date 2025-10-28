using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
{
    public class GetAllRoomTypeLogic : IDomainLogic<GetAllRoomTypeParam, Task<GetAllRoomTypeResultData>>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;

        public GetAllRoomTypeLogic(IRoomTypeRepository roomTypeRepository)
        {
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<GetAllRoomTypeResultData> Execute(GetAllRoomTypeParam param)
        {
            var roomTypes = await _roomTypeRepository.GetAllRoomType(param.Id, param.Type, param.BasePrice);

            return new GetAllRoomTypeResultData
            {
                Result = true,
                Message = "Get All room types Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = roomTypes.Select(c => new GetAllRoomTypesDataResult
                {
                    Type = c.Type,
                    BasePrice = c.BasePrice,
                }).ToList(),
            };
        }
    }
}
