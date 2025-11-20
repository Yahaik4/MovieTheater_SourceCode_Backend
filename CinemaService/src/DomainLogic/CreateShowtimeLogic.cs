using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using CinemaService.Infrastructure.EF.Models;
using ApiGateway.ServiceConnector.MovieService;

namespace CinemaService.DomainLogic
{
    public class CreateShowtimeLogic : IDomainLogic<CreateShowtimeParam, Task<CreateShowtimeResultData>>
    {
        private readonly IShowtimeRepository _showtimeRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IShowtimeSeatRepository _showtimeSeatRepository;
        private readonly MovieServiceConnector _movieServiceConnector;
        private readonly ISeatRepository _seatRepository;

        public CreateShowtimeLogic(IShowtimeRepository showtimeRepository, 
                                   IRoomRepository roomRepository, 
                                   IShowtimeSeatRepository showtimeSeatRepository,
                                   MovieServiceConnector movieServiceConnector,
                                   ISeatRepository seatRepository)
        {
            _showtimeRepository = showtimeRepository;
            _roomRepository = roomRepository;
            _showtimeSeatRepository = showtimeSeatRepository;
            _movieServiceConnector = movieServiceConnector; 
            _seatRepository = seatRepository;
        }

        public async Task<CreateShowtimeResultData> Execute(CreateShowtimeParam param)
        {
            var room = await _roomRepository.GetRoomById(param.RoomId);

            if (room == null) {
                throw new NotFoundException("Room not found");
            }

            var movie = await _movieServiceConnector.GetMovies(param.MovieId, null, null, null);
            if (movie == null || movie.Data == null || !movie.Data.Any()) {
                throw new NotFoundException("Movie not found");
            }
            
            var movieData = movie.Data.First();

            Guid showtimeId = Guid.NewGuid();
            
            var showtimes = await _showtimeRepository.GetShowtimesByRoomId(room.Id);

            TimeSpan buffer = TimeSpan.FromMinutes(15);

            bool isOverlap = showtimes.Any(st =>
                !st.IsDeleted &&
                (param.StartTime < st.EndTime.Add(buffer) && param.EndTime.Add(-buffer) > st.StartTime)
            );

            if (isOverlap)
                throw new ConflictException("Showtime conflicts with an existing showtime or buffer time in this room.");


            var newShowtime = await _showtimeRepository.CreateShowtime(new Showtime
            {
                Id = showtimeId,
                RoomId = room.Id,
                MovieId = Guid.Parse(movieData.Id),
                StartTime = param.StartTime,
                EndTime = param.EndTime,
                Status = param.Status,
            });

            if(param.Status.ToLower() == "open")
            {
                var seats = await _seatRepository.GetAllSeatByRoom(room.Id);
                
                if (seats.Any())
                {
                    var seatIds = seats.Where(s => s.isActive).Select(s => s.Id).ToList();
                    await _showtimeSeatRepository.CreateShowtimeSeats(showtimeId, seatIds, room.RoomType.BasePrice);
                }
            }

            return new CreateShowtimeResultData
            {
                Result = true,
                Message = "Create Showtime Successfully",
                StatusCode = StatusCodeEnum.Created,
                Data = new CreateShowtimeDataResult
                {
                    ShowtimeId = showtimeId,
                    RoomId = room.Id,
                    MovieId = newShowtime.MovieId,
                    StartTime = newShowtime.StartTime.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt"),
                    EndTime = newShowtime.EndTime.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt"),
                    MovieName = movieData.Name,
                    RoomNumber = room.RoomNumber,
                    Status = newShowtime.Status,
                }
            };
        }
    }
}
