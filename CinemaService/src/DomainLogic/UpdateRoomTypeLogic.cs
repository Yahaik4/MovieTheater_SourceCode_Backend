using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class UpdateRoomTypeLogic : IDomainLogic<UpdateRoomTypeParam, Task<UpdateRoomTypeResultData>>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        public UpdateRoomTypeLogic(IRoomTypeRepository roomTypeRepository)
        {
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<UpdateRoomTypeResultData> Execute(UpdateRoomTypeParam param)
        {
            var roomType = await _roomTypeRepository.GetRoomTypeById(param.Id);

            if (roomType == null)
            {
                throw new NotFoundException("Type Not Found");
            }

            roomType.Type = param.Type;
            roomType.ExtraPrice = param.ExtraPrice;
            roomType.UpdatedAt = DateTime.UtcNow;

            await _roomTypeRepository.UpdateRoomType(roomType);

            return new UpdateRoomTypeResultData
            {
                Result = true,
                StatusCode = StatusCodeEnum.Success,
                Message = "Update Cinema Successfully",
                Data = new UpdateRoomTypeDataResult
                {
                    Type = roomType.Type,
                    BasePrice = roomType.ExtraPrice,
                }
            };
        }
    }
}
