using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class GetShowtimesByMovieLogic : IDomainLogic<GetShowtimesByMovieParam, Task<GetShowtimesByMovieResultData>>
    {
        private readonly ICinemaRepository _cinemaRepository;

        public GetShowtimesByMovieLogic(ICinemaRepository cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
        }

        public async Task<GetShowtimesByMovieResultData> Execute(GetShowtimesByMovieParam param)
        {
            var cinemas = await _cinemaRepository.GetCinemasWithShowtimes(param.MovieId, param.Date, param.Country);

            return new GetShowtimesByMovieResultData
            {
                Result = true,
                Message = "Get Showtimes Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = cinemas.Where(c => c.Rooms.Any(r => r.Showtimes.Any()))
                              .Select(c => new GetShowtimesByMovieDataResult
                              {
                                    CinemaId = c.Id,
                                    CinemaName = c.Name,
                                    Address = c.Address,
                                    RoomTypes = c.Rooms.Where(r => r.Showtimes.Any())
                                                       .GroupBy(r => new { r.RoomTypeId, r.RoomType.Type })
                                                       .Select(g => new GetRoomTypeDataResult
                                                       {
                                                           RoomTypeId = g.Key.RoomTypeId,
                                                           RoomTypeName = g.Key.Type,
                                                           Showtimes = g.SelectMany(r => r.Showtimes)
                                                                        .Select(st => new ShowtimeDataResult
                                                                        {
                                                                            ShowtimeId = st.Id,
                                                                            StartTime = st.StartTime.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt"),
                                                                            EndTime = st.EndTime.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt")
                                                                        })
                                                                        .OrderBy(st => st.StartTime)
                                                                        .ToList()
                                                       }).ToList()
                              }).ToList()
            };
        }
    }
}
