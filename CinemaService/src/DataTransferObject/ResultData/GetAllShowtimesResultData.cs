using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class GetAllShowtimesResultData : BaseResultData
    {
        public List<GetAllShowtimesCinemaData> Data { get; set; } = new();
    }

    public class GetAllShowtimesCinemaData
    {
        public Guid CinemaId { get; set; }
        public string CinemaName { get; set; }
        public string Address { get; set; }

        public List<GetAllShowtimesRoomTypeData> RoomTypes { get; set; } = new();
    }

    public class GetAllShowtimesRoomTypeData
    {
        public Guid RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }

        public List<GetAllShowtimesShowtimeData> Showtimes { get; set; } = new();
    }

    public class GetAllShowtimesShowtimeData
    {
        public Guid ShowtimeId { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public Guid MovieId { get; set; }
        public string? MovieName { get; set; }
    }
}
