using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class GetShowtimeByRoomResultData : BaseResultData
    {
        public List<GetShowtimeByRoomDataResult> Data { get; set; }
    }

    public class GetShowtimeByRoomDataResult
    {
        public Guid ShowtimeId { get; set; }
        public string Status { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
    }
}
