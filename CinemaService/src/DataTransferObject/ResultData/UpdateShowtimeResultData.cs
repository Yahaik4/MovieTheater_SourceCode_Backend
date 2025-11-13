using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class UpdateShowtimeResultData : BaseResultData
    {
        public UpdateShowtimeDataResult Data { get; set; } = null!;
    }

    public class UpdateShowtimeDataResult
    {
        public Guid ShowtimeId { get; set; }
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public Guid RoomId { get; set; }
        public int RoomNumber { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Status { get; set; }
    }
}
