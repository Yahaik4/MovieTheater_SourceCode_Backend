using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class CreateRoomTypeResultData : BaseResultData
    {
        public CreateRoomTypeDataResult Data { get; set; } = null!;
    }

    public class CreateRoomTypeDataResult
    {
        public Guid Id { get; set; }
        public string Type { get; set; } // 2D, 3D, IMAX
        public decimal BasePrice { get; set; }
        public string CreatedBy { get; set; }
    }
}
