namespace src.DataTransferObject.ResultData
{
    public class CreateRoomResultDTO : BaseResultDTO
    {
        public CreateRoomDataResult Data { get; set; }
    }

    public class CreateRoomDataResult
    {
        public Guid Id { get; set; }
        public int RoomNumber { get; set; }
        public string Status { get; set; } // 
        public int TotalColumn { get; set; }
        public int TotalRow { get; set; }
        public string RoomType { get; set; }
        public string Cinema { get; set; }
        public string CreatedBy { get; set; }
        public List<CreateSeatDataResult> Seats { get; set; }
    }

    public class CreateSeatDataResult
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public int ColumnIndex { get; set; }
        public int DisplayNumber { get; set; }
        public string SeatCode { get; set; }
        public bool isActive { get; set; }
        public string Status { get; set; }
        public string SeatType { get; set; }
    }
}
