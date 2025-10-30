using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class UpdateRoomTypeParam : IParam
    { 
        public Guid Id { get; set; }
        public string Type { get; set; } // 2D, 3D, IMAX
        public decimal BasePrice { get; set; }
    }
}
