using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class GetShowtimesByCinemaResultData : BaseResultData
    {
        public List<GetShowtimesByCinemaDataResult> Data { get; set; }
    }

    public class GetShowtimesByCinemaDataResult
    {
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public string MovieDescription { get; set; }
        public string Poster { get; set; }
        public List<GetRoomTypeDataResult> RoomTypes { get; set; }
    }
}
