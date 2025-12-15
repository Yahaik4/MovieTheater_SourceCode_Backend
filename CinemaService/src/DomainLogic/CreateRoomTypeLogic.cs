using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using CinemaService.Helper;
using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.DomainLogic
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
                ExtraPrice = param.ExtraPrice,
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
                    BasePrice = roomType.ExtraPrice,
                    CreatedBy = roomType.CreatedBy,
                }
            };
        }
    }
}
