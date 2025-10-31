using Shared.Contracts.ResultData;

namespace src.DataTransferObject.ResultData
{
    public class UpdateRoomResultData : BaseResultData
    {
        public UpdateRoomDataResult Data { get; set; } = null!;
    }

    public class UpdateRoomDataResult
    {
        public int RoomNumber { get; set; }
        public string Status { get; set; }
        public int Total_Column { get; set; }
        public int Total_Row { get; set; }
        public string Type { get; set; }
    }
}
