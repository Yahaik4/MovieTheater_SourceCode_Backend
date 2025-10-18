using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
{
    public class CreateRoomsLogic : IDomainLogic<CreateListRoomParam, Task<CreateListRoomResultData>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ICinemaRepository _cinemaRepository;

        public CreateRoomsLogic(IRoomRepository roomRepository, ICinemaRepository cinemaRepository)
        {
            _roomRepository = roomRepository;
            _cinemaRepository = cinemaRepository;
        }

        public async Task<CreateListRoomResultData> Execute(CreateListRoomParam param)
        {
            if (param == null)
            {
                throw new ValidationException("Param cannot be blank.");
            }

            var cinema = await _cinemaRepository.GetCinemaById(param.CinemaId);

            if (cinema == null) {
                throw new NotFoundException("Cinema not found");
            }

            var existingRooms = await _roomRepository.GetAllRoom(param.CinemaId);
            var existingRoomsNumbers = existingRooms.Select(r => r.RoomNumber).ToHashSet();

            var newRooms = new List<Room>();
            var skippedRooms = new List<int>();

            foreach (var roomParam in param.Rooms)
            {
                if (existingRoomsNumbers.Contains(roomParam.RoomNumber))
                {
                    skippedRooms.Add(roomParam.RoomNumber);
                    continue;
                }

                var room = new Room
                {
                    Id = Guid.NewGuid(),
                    RoomNumber = roomParam.RoomNumber,
                    Type = roomParam.Type,
                    Status = roomParam.Status ?? "Active",
                    CinemaId = param.CinemaId,
                    LayoutId = roomParam.LayoutId ?? Guid.Empty,
                };

                newRooms.Add(room);
                existingRoomsNumbers.Add(room.RoomNumber);
            }

            if (!newRooms.Any()) {
                throw new ValidationException("All room is existed");
            }
            else
            {
                await _roomRepository.AddListRoom(newRooms);
            }

            string message;
            if (!skippedRooms.Any()) {
                message = "Add Rooms Successfully";
            }
            else
            {
                message = $"Some rooms were existed: {string.Join(", ", skippedRooms)}";
            }

            return new CreateListRoomResultData
            {
                Result = true,
                Message = message,
                StatusCode = StatusCodeEnum.Success,
                Data = new CreateListRoomDataResult
                {
                    CinemaId = param.CinemaId,
                    Rooms = newRooms.Select(r => new RoomDataResult
                    {
                        RoomNumber = r.RoomNumber,
                        Type = r.Type,
                        Status = r.Status,
                        LayoutId = r.LayoutId
                    }).ToList()
                }
            };
        }
    }
}
