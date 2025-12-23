using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
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
                    RoomTypeId = c.RoomType.Id,
                    RoomType = c.RoomType.Type,
                    CreatedBy = c.CreatedBy,
                }).ToList(),
            };
        }
    }
}
