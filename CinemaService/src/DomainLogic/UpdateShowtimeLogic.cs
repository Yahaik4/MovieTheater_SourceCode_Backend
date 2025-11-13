using ApiGateway.ServiceConnector.MovieService;
using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using MovieGrpc;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using System.Text.Json;

namespace CinemaService.DomainLogic
{
    public class UpdateShowtimeLogic : IDomainLogic<UpdateShowtimeParam, Task<UpdateShowtimeResultData>>
    {
        private readonly IShowtimeRepository _showtimeRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IShowtimeSeatRepository _showtimeSeatRepository;
        private readonly MovieServiceConnector _movieServiceConnector;
        private readonly ISeatRepository _seatRepository;

        public UpdateShowtimeLogic(IShowtimeRepository showtimeRepository,
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

        public async Task<UpdateShowtimeResultData> Execute(UpdateShowtimeParam param)
        {
            Console.WriteLine("=== Received UpdateShowtimeParam ===");
            Console.WriteLine(JsonSerializer.Serialize(param, new JsonSerializerOptions
            {
                WriteIndented = true
            }));

            var showtime = await _showtimeRepository.GetShowtimeById(param.Id);

            if (showtime == null)
            {
                throw new NotFoundException("Showtime not found");
            }

            GetMoviesGrpcReplyDataDTO? movieData = null;
            if (param.MovieId.HasValue)
            {
                var movie = await _movieServiceConnector.GetMovies(param.MovieId.Value, null, null, null);
                if (movie == null || movie.Data == null || !movie.Data.Any())
                {
                    throw new NotFoundException("Movie not found");
                }

                movieData = movie.Data.First();
                showtime.MovieId = param.MovieId.Value;
            }
            else
            {
                Console.WriteLine("movieId: ", showtime.MovieId);
                var movie = await _movieServiceConnector.GetMovies(showtime.MovieId, null, null, null);
                Console.WriteLine("movie: ", movie);

                Console.WriteLine("=== Movie Response ===");
                Console.WriteLine(JsonSerializer.Serialize(movie, new JsonSerializerOptions
                {
                    WriteIndented = true // in đẹp, dễ đọc
                }));

                movieData = movie.Data.First();
            }

            if (param.RoomId.HasValue)
            {
                var room = await _roomRepository.GetRoomById(param.RoomId.Value);
                if (room == null)
                    throw new NotFoundException("Room not found");

                showtime.RoomId = param.RoomId.Value;
            }

            if (param.StartTime.HasValue)
                showtime.StartTime = param.StartTime.Value;

            if (param.EndTime.HasValue)
                showtime.EndTime = param.EndTime.Value;

            if (param.StartTime.HasValue || param.EndTime.HasValue || param.RoomId.HasValue)
            {
                var buffer = TimeSpan.FromMinutes(15);

                var showtimesInRoom = await _showtimeRepository.GetShowtimesByRoomId(showtime.RoomId);

                bool isOverlap = showtimesInRoom.Any(st =>
                    st.Id != showtime.Id &&
                    !st.IsDeleted &&
                    (
                        showtime.StartTime < st.EndTime.Add(buffer) &&
                        showtime.EndTime.Add(-buffer) > st.StartTime
                    )
                );

                if (isOverlap)
                    throw new ConflictException("Showtime conflicts with an existing showtime or buffer time in this room.");
            }

            if (!string.IsNullOrEmpty(param.Status))
            {
                if (param.Status.ToLower() == "open")
                {
                    var seats = await _seatRepository.GetAllSeatByRoom(showtime.RoomId);

                    if (seats.Any())
                    {
                        var seatIds = seats.Where(s => s.isActive).Select(s => s.Id).ToList();
                        await _showtimeSeatRepository.CreateShowtimeSeats(showtime.Id, seatIds);
                    }
                }

                if(param.Status.ToLower() == "completed" ||  param.Status.ToLower() == "cancelled")
                {
                    await _showtimeSeatRepository.DeleteShowtimeSeatsByShowtimeId(showtime.Id);
                }

                showtime.Status = param.Status;
            }

            await _showtimeRepository.UpdateShowtime(showtime);
            
            return new UpdateShowtimeResultData
            {
                Result = true,
                Message = "Update Showtime Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new UpdateShowtimeDataResult
                {
                    ShowtimeId = showtime.Id,
                    RoomId = showtime.RoomId,
                    MovieId = showtime.MovieId,
                    StartTime = showtime.StartTime.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt"),
                    EndTime = showtime.EndTime.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt"),
                    MovieName = movieData.Name,
                    Status = showtime.Status,
                    RoomNumber = showtime.Room.RoomNumber
                }
            };
        }
    }
}
