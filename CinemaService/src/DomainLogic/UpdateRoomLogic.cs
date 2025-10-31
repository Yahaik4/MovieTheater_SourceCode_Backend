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

            var isRoomExists = rooms.Any(r => r.RoomNumber == param.RoomNumber);

            if (isRoomExists)
            {
                throw new ValidationException($"Room number {param.RoomNumber} already exists in this cinema.");
            }

            var roomType = await _roomTypeRepository.GetRoomTypeById(param.Id);

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

                foreach (var seat in seatsToRemove)
                {
                    await _seatRepository.DeleteSeat(seat);
                }
            }

            if (param.Total_Row > oldTotalRow || param.Total_Column > oldTotalColumn)
            {
                var newSeats = new List<Seat>();
                var seatType = await _seatTypeRepository.GetSeatTypeByName("Standard");

                for (int r = 0; r < param.Total_Row; r++)
                {
                    char rowLabel = (char)('A' + r);
                    for (int c = 1; c <= param.Total_Column; c++)
                    {
                        if (seats.Any(s => s.Label == rowLabel.ToString() && s.ColumnIndex == c))
                            continue;

                        newSeats.Add(new Seat
                        {
                            Id = Guid.NewGuid(),
                            RoomId = room.Id,
                            Label = rowLabel.ToString(),
                            ColumnIndex = c,
                            DisplayNumber = c,
                            SeatCode = $"{rowLabel}{c}",
                            SeatTypeId = seatType.Id,
                            isActive = true,
                            Status = "Active",
                            CreatedAt = DateTime.UtcNow,
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
                    Total_Row = room.Total_Row,
                    Total_Column = room.Total_Column,
                    Status = room.Status,
                    Type = room.GetType().Name,
                }
            };
        }
    }
}
