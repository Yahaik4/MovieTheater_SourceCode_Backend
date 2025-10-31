namespace src.DataTransferObject.ResultData
{
    public class UpdateRoomResultDTO : BaseResultDTO
    {
        public UpdateRoomDataResult Data { get; set; }
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
