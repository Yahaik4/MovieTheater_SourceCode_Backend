using Shared.Contracts.ResultData;

namespace src.DataTransferObject.ResultData
{
    public class CreateRoomResultData : BaseResultData
    {
        public CreateRoomDataResult Data { get; set; }
    }

    public class CreateRoomDataResult
    {
        public int RoomNumber { get; set; }
        public string Status { get; set; } // 
        public int Total_Column { get; set; }
        public int Total_Row { get; set; }
        public string RoomType { get; set; }
        public Guid CinemaId { get; set; }
    }
}
