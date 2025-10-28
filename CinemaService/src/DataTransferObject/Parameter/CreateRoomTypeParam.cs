using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class CreateRoomTypeParam : IParam
    {
        public string Type { get; set; } // 2D, 3D, IMAX
        public decimal BasePrice { get; set; }
        public string CreatedBy { get; set; }
    }
}
