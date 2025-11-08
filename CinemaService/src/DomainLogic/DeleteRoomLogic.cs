using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class DeleteRoomLogic : IDomainLogic<DeleteRoomParam, Task<DeleteRoomResultData>>
    {
        private readonly IRoomRepository _roomRepository;
        public DeleteRoomLogic(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<DeleteRoomResultData> Execute(DeleteRoomParam param)
        {
            var room = await _roomRepository.GetRoomById(param.Id);

            if (room == null)
            {
                throw new NotFoundException("Room Not Found");
            }

            room.IsDeleted = true;

            await _roomRepository.UpdateRoom(room);

            return new DeleteRoomResultData
            {
                Result = true,
                StatusCode = StatusCodeEnum.Success,
                Message = "Delete Room Successfully"
            };
        }
    }
}
