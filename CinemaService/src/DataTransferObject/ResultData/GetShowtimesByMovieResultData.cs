using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class GetShowtimesByMovieResultData : BaseResultData
    {
        public List<GetShowtimesByMovieDataResult> Data { get; set; }
    }

    public class GetShowtimesByMovieDataResult
    {
        public Guid CinemaId { get; set; }
        public string CinemaName { get; set; }
        public string Address { get; set; }
        public List<GetRoomTypeDataResult> RoomTypes { get; set; }
    }

    public class GetRoomTypeDataResult
    {
        public Guid RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public List<ShowtimeDataResult> Showtimes { get; set; }
    }

    public class ShowtimeDataResult
    {
        public Guid ShowtimeId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

}
