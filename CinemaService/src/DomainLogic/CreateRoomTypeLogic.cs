using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Helper;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
{
    public class CreateRoomTypeLogic : IDomainLogic<CreateRoomTypeParam, Task<CreateRoomTypeResultData>>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;

        public CreateRoomTypeLogic(IRoomTypeRepository roomTypeRepository)
        {
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<CreateRoomTypeResultData> Execute(CreateRoomTypeParam param)
        {
            if (param == null)
            {
                throw new ValidationException("Param cannot be blank.");
            }

            var roomType = new RoomType
            {
                Id = Guid.NewGuid(),
                Type = param.Type,
                BasePrice = param.BasePrice,
                CreatedBy = param.CreatedBy,
            };

            await _roomTypeRepository.CreateRoomType(roomType);

            return new CreateRoomTypeResultData
            {
                Result = true,
                Message = "Create new Room Type Successfully",
                StatusCode = StatusCodeEnum.Created,
                Data = new CreateRoomTypeDataResult
                {
                    Id = roomType.Id,
                    Type = roomType.Type,
                    BasePrice = roomType.BasePrice,
                    CreatedBy = param.CreatedBy,
                }
            };
        }
    }
}
