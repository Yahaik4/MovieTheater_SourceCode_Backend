using ApiGateway.ServiceConnector.MovieService;
using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class GetShowtimesByCinemaLogic : IDomainLogic<GetShowtimesByCinemaParam, Task<GetShowtimesByCinemaResultData>>
    {
        private readonly IShowtimeRepository _showtimeRepository;
        private readonly MovieServiceConnector _movieServiceConnector;

        public GetShowtimesByCinemaLogic(IShowtimeRepository showtimeRepository, MovieServiceConnector movieServiceConnector)
        {
            _showtimeRepository = showtimeRepository;
            _movieServiceConnector = movieServiceConnector;
        }

        public async Task<GetShowtimesByCinemaResultData> Execute(GetShowtimesByCinemaParam param)
        {
            var startDate = DateTime.SpecifyKind(param.Date.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
            var endDate = DateTime.SpecifyKind(param.Date.ToDateTime(TimeOnly.MaxValue), DateTimeKind.Utc);

            var showtimes = await _showtimeRepository.GetShowtimesByCinemaAndDate(param.CinemaId, startDate, endDate);

            if (!showtimes.Any())
            {
                return new GetShowtimesByCinemaResultData
                {
                    Result = true,
                    StatusCode = StatusCodeEnum.Success,
                    Message = "No movies playing",
                    Data = new List<GetShowtimesByCinemaDataResult>()
                };
            }

            var movieIds = showtimes.Select(s => s.MovieId).Distinct().ToList();

            var reply = await _movieServiceConnector.GetMovieByIds(movieIds);

            Console.WriteLine("=== DEBUG reply ===");
            Console.WriteLine(reply == null ? "reply NULL" : "reply OK");
            Console.WriteLine(reply?.Data == null ? "reply.Data NULL" : $"reply.Data Count = {reply.Data.Count}");
            Console.WriteLine("====================");

            var final = movieIds.Select(movieId =>
            {
                var movie = reply.Data?
                    .FirstOrDefault(m =>
                        !string.IsNullOrWhiteSpace(m.Id) &&
                        Guid.TryParse(m.Id, out var gid) &&
                        gid == movieId
                    );

                var showtimesOfMovie = showtimes
                    .Where(s => s != null && s.MovieId == movieId)
                    .ToList();

                return new GetShowtimesByCinemaDataResult
                {
                    MovieId = movieId,
                    MovieName = movie?.Name,
                    Poster = movie?.Poster,
                    MovieDescription = movie?.Description,

                    RoomTypes = showtimesOfMovie
                        .Where(st => st.Room != null && st.Room.RoomType != null)
                        .GroupBy(st => st.Room.RoomTypeId)
                        .Select(g => new GetRoomTypeDataResult
                        {
                            RoomTypeId = g.Key,
                            RoomTypeName = g.First().Room.RoomType?.Type ?? "Unknown",

                            Showtimes = g
                                .Where(st => st != null)
                                .Select(st => new ShowtimeDataResult
                                {
                                    ShowtimeId = st.Id,
                                    StartTime = st.StartTime.ToLocalTime().ToString("MM/dd/yyyy HH:mm"),
                                    EndTime = st.EndTime.ToLocalTime().ToString("MM/dd/yyyy HH:mm")
                                })
                                .OrderBy(x => x.StartTime)
                                .ToList()
                        })
                        .ToList()
                };
            }).ToList();

            return new GetShowtimesByCinemaResultData
            {
                Result = true,
                Message = "Get Showtimes By Cinema Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = final
            };

        }
    }
}
