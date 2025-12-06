using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class GetAllShowtimesLogic : IDomainLogic<GetAllShowtimesParam, Task<GetAllShowtimesResultData>>
    {
        private readonly IShowtimeRepository _showtimeRepository;

        public GetAllShowtimesLogic(IShowtimeRepository showtimeRepository)
        {
            _showtimeRepository = showtimeRepository;
        }

        public async Task<GetAllShowtimesResultData> Execute(GetAllShowtimesParam param)
        {
            var showtimes = await _showtimeRepository
                .GetAllShowtimesAsync(param.CinemaId, param.MovieId, param.Date);

            var cinemas = showtimes
                .GroupBy(st => st.Room.Cinema)
                .Select(cinemaGroup => new GetAllShowtimesCinemaData
                {
                    CinemaId = cinemaGroup.Key.Id,
                    CinemaName = cinemaGroup.Key.Name,
                    Address = cinemaGroup.Key.Address,
                    RoomTypes = cinemaGroup
                        .GroupBy(st => st.Room.RoomType)
                        .Select(rtGroup => new GetAllShowtimesRoomTypeData
                        {
                            RoomTypeId = rtGroup.Key.Id,
                            RoomTypeName = rtGroup.Key.Type,
                            Showtimes = rtGroup
                                .Select(st => new GetAllShowtimesShowtimeData
                                {
                                    ShowtimeId = st.Id,
                                    StartTime = st.StartTime.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt"),
                                    EndTime = st.EndTime.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt"),
                                    MovieId = st.MovieId,
                                    MovieName = null
                                })
                                .OrderBy(x => x.StartTime)
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            return new GetAllShowtimesResultData
            {
                Result = true,
                Message = "Get all showtimes successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = cinemas
            };
        }
    }
}
