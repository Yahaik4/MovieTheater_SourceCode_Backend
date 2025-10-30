using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class CreateRoomParam : IParam
    {
        public int RoomNumber { get; set; }
        public string Status { get; set; } // 
        public int Total_Column { get; set; }
        public int Total_Row { get; set; }
        public Guid RoomTypeId { get; set; }
        public Guid CinemaId { get; set; }
    }
}
