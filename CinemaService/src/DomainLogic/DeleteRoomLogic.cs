using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
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
