namespace ApiGateway.DataTransferObject.ResultData
{
    public class UpdateRoomResultDTO : BaseResultDTO
    {
        public UpdateRoomDataResult Data { get; set; }
    }

    public class UpdateRoomDataResult
    {
        public Guid Id { get; set; }
        public int RoomNumber { get; set; }
        public string Status { get; set; } // 
        public int TotalColumn { get; set; }
        public int TotalRow { get; set; }
        public string RoomType { get; set; }
        public string Cinema { get; set; }
    }
}
