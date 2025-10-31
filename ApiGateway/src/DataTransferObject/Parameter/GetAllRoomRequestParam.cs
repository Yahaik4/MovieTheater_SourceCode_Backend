namespace src.DataTransferObject.Parameter
{
    public class GetAllRoomRequestParam
    {
        public Guid? Id { get; set; }
        public int? RoomNumber { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
    }
}
