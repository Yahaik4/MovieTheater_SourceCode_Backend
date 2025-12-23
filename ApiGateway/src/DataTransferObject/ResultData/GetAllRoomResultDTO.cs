namespace ApiGateway.DataTransferObject.ResultData
{
    public class GetAllRoomResultDTO : BaseResultDTO
    {
        public List<GetAllRoomsDataResult> Data { get; set; }
    }

    public class GetAllRoomsDataResult
    {
        public Guid Id { get; set; }
        public int RoomNumber { get; set; }
        public string Status { get; set; }
        public int TotalColumn { get; set; }
        public int TotalRow { get; set; }
        public Guid RoomTypeId { get; set; }
        public string RoomType { get; set; }
        public string CreatedBy { get; set; }
    }
}
