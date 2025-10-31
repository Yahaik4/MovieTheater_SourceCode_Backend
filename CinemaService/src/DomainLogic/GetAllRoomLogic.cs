using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
{
    public class GetAllRoomLogic : IDomainLogic<GetAllRoomParam, Task<GetAllRoomResultData>>
    {
        private readonly IRoomRepository _roomRepository;

        public GetAllRoomLogic(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<GetAllRoomResultData> Execute(GetAllRoomParam param)
        {
            var rooms = await _roomRepository.GetAllRoomByCinema(param.CinemaId, param.Id, param.RoomNumber, param.Status, param.Type);

            return new GetAllRoomResultData
            {
                Result = true,
                Message = "Get All Cinemas Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = rooms.Select(c => new GetAllRoomDataResult
                {
                    Id = c.Id,
                    RoomNumber = c.RoomNumber,
                    TotalColumn = c.Total_Column,
                    TotalRow = c.Total_Row,
                    Status = c.Status,
                    RoomType = c.RoomType.Type,
                    CreatedBy = c.CreatedBy,
                }).ToList(),
            };
        }
    }
}
