using ApiGateway.ServiceConnector.MovieService;
using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class GetShowtimeByRoomLogic : IDomainLogic<GetShowtimeByRoomParam, Task<GetShowtimeByRoomResultData>>
    {
        private readonly IShowtimeRepository _showtimeRepository;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly MovieServiceConnector _movieServiceConnector;
        private readonly IRoomTypeRepository _roomTypeRepository;

        public GetShowtimeByRoomLogic(IShowtimeRepository showtimeRepository, ICinemaRepository cinemaRepository, MovieServiceConnector movieServiceConnector, IRoomTypeRepository roomTypeRepository)
        {
            _showtimeRepository = showtimeRepository;
            _cinemaRepository = cinemaRepository;
            _movieServiceConnector = movieServiceConnector;
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<GetShowtimeByRoomResultData> Execute(GetShowtimeByRoomParam param)
        {
            var showtimes = await _showtimeRepository.GetShowtimesByRoomIdInDateRange(param.RoomId, param.From, param.To);

            if (!showtimes.Any())
            {
                return new GetShowtimeByRoomResultData
                {
                    Result = true,
                    Message = "No showtime found",
                    StatusCode = StatusCodeEnum.Success,
                    Data = new List<GetShowtimeByRoomDataResult>()
                };
            }

            var movieIds = showtimes.Select(s => s.MovieId).Distinct().ToList();
            var movies = await _movieServiceConnector.GetMovieByIds(movieIds);
            var movieDict = movies.Data.ToDictionary(m => Guid.Parse(m.Id), m => m);

            var data = new List<GetShowtimeByRoomDataResult>();

            foreach (var s in showtimes)
            {
                movieDict.TryGetValue(s.MovieId, out var movie);

                data.Add(new GetShowtimeByRoomDataResult
                {
                    ShowtimeId = s.Id,
                    Status = s.Status,
                    StartTime = s.StartTime.ToString(),
                    EndTime = s.EndTime.ToString(),
                    MovieId = s.MovieId,
                    MovieName = movie?.Name ?? string.Empty,
                });
            }

            return new GetShowtimeByRoomResultData
            {
                Result = true,
                Message = "Get Showtimes Success",
                StatusCode = StatusCodeEnum.Success,
                Data = data
            };
        }
    }
}
