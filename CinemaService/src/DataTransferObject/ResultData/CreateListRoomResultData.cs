using Shared.Contracts.ResultData;
using src.Infrastructure.EF.Models;

namespace src.DataTransferObject.ResultData
{
    public class CreateListRoomResultData : BaseResultData
    {
        public CreateListRoomDataResult Data { get; set; } = null!;
    }

    public class CreateListRoomDataResult
    {
        public Guid CinemaId { get; set; }
        public List<RoomDataResult> Rooms { get; set; } 
    }

    public class RoomDataResult
    {
        public int RoomNumber { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public Guid? LayoutId { get; set; }
    } 
}
