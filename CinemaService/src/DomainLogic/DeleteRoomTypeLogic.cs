using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class DeleteRoomTypeLogic : IDomainLogic<DeleteRoomTypeParam, Task<DeleteRoomTypeResultData>>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        public DeleteRoomTypeLogic(IRoomTypeRepository roomTypeRepository) 
        {
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<DeleteRoomTypeResultData> Execute(DeleteRoomTypeParam param)
        {
            var roomType = await _roomTypeRepository.GetRoomTypeById(param.Id);

            if (roomType == null)
            {
                throw new NotFoundException("Type Not Found");
            }

            roomType.IsDeleted = true;

            await _roomTypeRepository.UpdateRoomType(roomType);

            return new DeleteRoomTypeResultData
            {
                Result = true,
                StatusCode = StatusCodeEnum.Success,
                Message = "Delete Successfully",
            };
        }
    }
}
