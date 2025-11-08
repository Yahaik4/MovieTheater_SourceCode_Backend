using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class GetAllRoomResultData : BaseResultData
    {
        public List<GetAllRoomDataResult> Data { get; set; }
    }

    public class GetAllRoomDataResult
    {
        public Guid Id { get; set; }
        public int RoomNumber { get; set; }
        public string Status { get; set; }
        public int TotalColumn { get; set; }
        public int TotalRow { get; set; }
        public string RoomType { get; set; }
        public string CreatedBy { get; set; }
    }
}
