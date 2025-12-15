using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class CreateRoomTypeParam : IParam
    {
        public string Type { get; set; } // 2D, 3D, IMAX
        public decimal ExtraPrice { get; set; }
        public string CreatedBy { get; set; }
    }
}
