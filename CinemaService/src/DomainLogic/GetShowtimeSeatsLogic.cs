using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class GetShowtimeSeatsLogic : IDomainLogic<GetShowtimeSeatsParam, Task<GetShowtimeSeatsResultData>>
    {
        private readonly IShowtimeSeatRepository _showtimeSeatRepository;
        private readonly IShowtimeRepository _showtimeRepository;

        public GetShowtimeSeatsLogic(IShowtimeSeatRepository showtimeSeatRepository, IShowtimeRepository showtimeRepository)
        {
            _showtimeSeatRepository = showtimeSeatRepository;
            _showtimeRepository = showtimeRepository;
        }

        public async Task<GetShowtimeSeatsResultData> Execute(GetShowtimeSeatsParam param)
        {
            var showtime = await _showtimeRepository.GetShowtimeById(param.showtimeId);

            if (showtime == null) {
                throw new NotFoundException("Showtime Not Found");
            }
            
            var showtimeSeats = await _showtimeSeatRepository.GetShowtimeSeatsByShowtimeId(param.showtimeId);

            return new GetShowtimeSeatsResultData
            {
                Result = true,
                Message = "Get ShowtimeSeats Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = showtimeSeats.Select(sts => new GetShowtimeSeatsDataResult
                {
                    ShowtimeSeatId = sts.Id,
                    RoomNumber = sts.Showtime.Room.RoomNumber,
                    Status = sts.Status,
                    SeatCode = sts.Seat.SeatCode,
                    Label = sts.Seat.Label,
                    Price = sts.Price,
                    SeatType = sts.Seat.SeatType.Type,
                }).ToList()
            };
        }
    }
}
