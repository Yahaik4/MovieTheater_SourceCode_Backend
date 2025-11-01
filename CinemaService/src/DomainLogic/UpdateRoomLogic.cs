using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
{
    public class UpdateRoomLogic : IDomainLogic<UpdateRoomParam, Task<UpdateRoomResultData>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ISeatTypeRepository _seatTypeRepository;
        public UpdateRoomLogic(IRoomRepository roomRepository, IRoomTypeRepository roomTypeRepository, ISeatRepository seatRepository, ISeatTypeRepository seatTypeRepository)
        {
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
            _seatRepository = seatRepository;
            _seatTypeRepository = seatTypeRepository;
        }

        public async Task<UpdateRoomResultData> Execute(UpdateRoomParam param)
        {
            var rooms = await _roomRepository.GetAllRoomByCinema(param.CinemaId, null, null, null, null);

            var isRoomExists = rooms.Any(r => r.RoomNumber == param.RoomNumber && r.Id != param.Id);

            if (isRoomExists)
            {
                throw new ValidationException($"Room number {param.RoomNumber} already exists in this cinema.");
            }

            var roomType = await _roomTypeRepository.GetRoomTypeById(param.RoomTypeId);

            if (roomType == null)
            {
                throw new NotFoundException("Type Not Found");
            }

            var room = await _roomRepository.GetRoomById(param.Id);

            if (room == null) { 
                throw new NotFoundException("Room Not Found");
            }

            var oldTotalRow = room.Total_Row;
            var oldTotalColumn = room.Total_Column;

            room.RoomNumber = param.RoomNumber;
            room.RoomTypeId = param.RoomTypeId;
            room.Status = param.Status;
            room.Total_Row = param.Total_Row;
            room.Total_Column = param.Total_Column;
            room.UpdatedAt = DateTime.UtcNow;

            await _roomRepository.UpdateRoom(room);

            var seats = await _seatRepository.GetAllSeatByRoom(room.Id);

            if (param.Total_Row < oldTotalRow || param.Total_Column < oldTotalColumn)
            {
                char maxRow = (char)('A' + param.Total_Row - 1);

                var seatsToRemove = seats.Where(s =>
                    s.Label[0] > maxRow ||
                    s.ColumnIndex > param.Total_Column
                ).ToList();

                if (seatsToRemove.Any())
                {
                    await _seatRepository.DeleteSeats(seatsToRemove);
                }
            }

            if (param.Total_Row > oldTotalRow || param.Total_Column > oldTotalColumn)
            {
                var newSeats = new List<Seat>();
                var seatType = await _seatTypeRepository.GetSeatTypeByName("Standard");

                for (int row = 0; row < param.Total_Row; row++)
                {
                    char rowLetter = (char)('A' + row);

                    for (int col = 1; col <= param.Total_Column; col++)
                    {
                        if (seats.Any(s => s.Label == $"{rowLetter}{col}"))
                            continue;

                        newSeats.Add(new Seat
                        {
                            Id = Guid.NewGuid(),
                            RoomId = room.Id,
                            Label = $"{rowLetter}{col}",
                            ColumnIndex = col,
                            DisplayNumber = (row * param.Total_Column) + col,
                            SeatCode = $"{room.RoomNumber}-{rowLetter}{col}",
                            SeatTypeId = seatType.Id,
                            isActive = true,
                            Status = "Available",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            IsDeleted = false
                        });
                    }
                }

                if (newSeats.Count > 0)
                    await _seatRepository.CreateSeats(newSeats);
            }


            return new UpdateRoomResultData
            {
                Result = true,
                Message = "Update Room Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new UpdateRoomDataResult
                {
                    RoomNumber = room.RoomNumber,
                    TotalRow = room.Total_Row,
                    TotalColumn = room.Total_Column,
                    Status = room.Status,
                    Type = room.RoomType.Type,
                }
            };
        }
    }
}
