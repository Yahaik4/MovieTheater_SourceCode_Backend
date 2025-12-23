using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class CreateRoomsLogic : IDomainLogic<CreateRoomParam, Task<CreateRoomResultData>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly ISeatTypeRepository _seatTypeRepository;
        private readonly ISeatRepository _seatRepository;

        public CreateRoomsLogic(IRoomRepository roomRepository, 
                                ICinemaRepository cinemaRepository, 
                                IRoomTypeRepository roomTypeRepository, 
                                ISeatTypeRepository seatTypeRepository,
                                ISeatRepository seatRepository)
        {
            _roomRepository = roomRepository;
            _cinemaRepository = cinemaRepository;
            _roomTypeRepository = roomTypeRepository;
            _seatTypeRepository = seatTypeRepository;
            _seatRepository = seatRepository;
        }

        public async Task<CreateRoomResultData> Execute(CreateRoomParam param)
        {
            if (param.RoomNumber <= 0)
            {
                throw new ValidationException("Room number must be greater than 0.");
            }

            var cinema = await _cinemaRepository.GetCinemaById(param.CinemaId);

            if (cinema == null)
            {
                throw new NotFoundException("Cinema not Found");
            }

            var type = await _roomTypeRepository.GetRoomTypeById(param.RoomTypeId);

            if (type == null)
            {
                throw new NotFoundException("Type not Found");
            }

            var rooms = await _roomRepository.GetAllRoomByCinema(param.CinemaId, null, null, null, null);

            var isRoomExists = rooms.Any(r => r.RoomNumber == param.RoomNumber);

            if (isRoomExists)
            {
                throw new ValidationException($"Room number {param.RoomNumber} already exists in this cinema.");
            }

            var newRoom = new Room
            {
                Id = Guid.NewGuid(),
                RoomNumber = param.RoomNumber,
                Status = param.Status,
                Total_Column = param.Total_Column,
                Total_Row = param.Total_Row,
                RoomTypeId = param.RoomTypeId,
                CinemaId = cinema.Id,
                CreatedBy = param.CreatedBy
            };

            await _roomRepository.CreateRoom(newRoom);

            var standardSeatType = await _seatTypeRepository.GetSeatTypeByName("Standard");

            if (standardSeatType == null)
            {
                throw new Exception("SeatType 'Standard' chưa được khởi tạo trong hệ thống.");
            }

            await GenerateSeats(newRoom, standardSeatType.Id);

            return new CreateRoomResultData
            {
                Result = true,
                Message = "Create new room successfully",
                StatusCode = StatusCodeEnum.Created,
                Data = new CreateRoomDataResult
                {
                    Id = newRoom.Id,
                    RoomNumber = newRoom.RoomNumber,
                    Status = newRoom.Status,
                    TotalColumn = newRoom.Total_Column,
                    TotalRow = newRoom.Total_Row,
                    RoomType = type.Type,
                    Cinema = cinema.Name,
                    CreatedBy = newRoom.CreatedBy,
                    Seats = newRoom.Seats.Select(s => new CreateSeatDataResult
                    {
                        Id = s.Id,
                        Label = s.Label,
                        ColumnIndex = s.ColumnIndex,
                        DisplayNumber = s.DisplayNumber,
                        SeatCode = s.SeatCode,
                        isActive = s.isActive,
                        Status = s.Status,
                        SeatType = standardSeatType.Type,
                    }).ToList()
                }
            };
        }

        private async Task<List<Seat>> GenerateSeats(Room room, Guid seatTypeId)
        {

            var seats = new List<Seat>();
            for (int row = 0; row < room.Total_Row; row++) 
            {
                char rowLetter = (char)('A' + row);

                for (int col = 1; col <= room.Total_Column; col++) 
                {
                    seats.Add(new Seat
                    {
                        Id = Guid.NewGuid(),
                        Label = $"{rowLetter}{col}",
                        ColumnIndex = col,
                        DisplayNumber = row * room.Total_Column + col,
                        SeatCode = $"{room.RoomNumber}-{rowLetter}{col}",
                        isActive = true,
                        Status = "available",
                        SeatTypeId = seatTypeId,
                        RoomId = room.Id
                    });
                }
            }

            await _seatRepository.CreateSeats(seats);

            return seats;
        }
    }
}
