using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class GetShowtimesLogic : IDomainLogic<GetShowtimesParam, Task<GetShowtimesResultData>>
    {
        private readonly IShowtimeSeatRepository _showtimeRepository;
        private readonly ICinemaRepository _cinemaRepository;

        public GetShowtimesLogic(IShowtimeSeatRepository showtimeRepository, ICinemaRepository cinemaRepository)
        {
            _showtimeRepository = showtimeRepository;
            _cinemaRepository = cinemaRepository;
        }

        public async Task<GetShowtimesResultData> Execute(GetShowtimesParam param)
        {
            var cinemas = await _cinemaRepository.GetCinemasWithShowtimes(param.MovieId, param.Date, param.Country);

            return new GetShowtimesResultData
            {
                Result = true,
                Message = "Get Showtimes Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = cinemas.Where(c => c.Rooms.Any(r => r.Showtimes.Any()))
                              .Select(c => new GetShowtimesDataResult
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
