using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
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
            var roomTypes = await _roomTypeRepository.GetAllRoomType(param.Id, param.Type, param.ExtraPrice);

            return new GetAllRoomTypeResultData
            {
                Result = true,
                Message = "Get All room types Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = roomTypes.Select(c => new GetAllRoomTypesDataResult
                {
                    Id = c.Id,
                    Type = c.Type,
                    BasePrice = c.ExtraPrice,
                }).ToList(),
            };
        }
    }
}
