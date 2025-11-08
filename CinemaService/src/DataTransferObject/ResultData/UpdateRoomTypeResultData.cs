using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class UpdateRoomTypeResultData : BaseResultData
    {
        public UpdateRoomTypeDataResult Data { get; set; } = null!;
    }

    public class UpdateRoomTypeDataResult
    {
        public string Type { get; set; } // 2D, 3D, IMAX
        public decimal BasePrice { get; set; }
    }
}
