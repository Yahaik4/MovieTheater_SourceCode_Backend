using ApiGateway.ServiceConnector.MovieService;
using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class GetShowtimeDetailsLogic : IDomainLogic<GetShowtimeDetailsParam, Task<GetShowtimeDetailsResultData>>
    {
        private readonly IShowtimeRepository _showtimeRepository;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly MovieServiceConnector _movieServiceConnector;
        private readonly IRoomTypeRepository _roomTypeRepository;

        public GetShowtimeDetailsLogic(IShowtimeRepository showtimeRepository, ICinemaRepository cinemaRepository, MovieServiceConnector movieServiceConnector, IRoomTypeRepository roomTypeRepository)
        {
            _showtimeRepository = showtimeRepository;
            _cinemaRepository = cinemaRepository;
            _movieServiceConnector = movieServiceConnector;
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<GetShowtimeDetailsResultData> Execute(GetShowtimeDetailsParam param)
        {
            var showtime = await _showtimeRepository.GetShowtimeById(param.ShowtimeId);

            if(showtime == null)
            {
                throw new NotFoundException("Showtime not found");
            }

            var cinema = await _cinemaRepository.GetCinemaById(showtime.Room.CinemaId);

            if (cinema == null)
            {
                throw new NotFoundException("Cinema not found");
            }

            var roomType = await _roomTypeRepository.GetRoomTypeById(showtime.Room.RoomTypeId);

            if (roomType == null)
            {
                throw new NotFoundException("Type room not found");
            }

            var movie = await _movieServiceConnector.GetMovies(showtime.MovieId, null, null, null);

            if (movie == null || movie.Data == null || !movie.Data.Any())
            {
                throw new NotFoundException("Movie not found");
            }

            return new GetShowtimeDetailsResultData
            {
                Result = true,
                Message = "Get ShowtimeDetails Success",
                StatusCode = StatusCodeEnum.Success,
                Data = new GetShowtimeDetailsDataResult
                {
                    CinemaId = cinema.Id,
                    CinemaName = cinema.Name,
                    City = cinema.City,
                    MovieId = Guid.Parse(movie.Data.First().Id),
                    MovieName = movie.Data.First().Name,
                    Poster = movie.Data.First().Poster,
                    RoomId = showtime.Room.Id,
                    RoomNumber = showtime.Room.RoomNumber,
                    TotalColumn = showtime.Room.Total_Column,
                    TotalRow = showtime.Room.Total_Row,
                    RoomType = roomType.Type,
                    StartTime = showtime.StartTime,
                    EndTime = showtime.EndTime
                }
            };
        }
    }
}
