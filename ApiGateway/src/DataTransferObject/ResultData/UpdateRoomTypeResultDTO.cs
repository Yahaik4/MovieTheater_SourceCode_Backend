namespace ApiGateway.DataTransferObject.ResultData
{
    public class UpdateRoomTypeResultDTO : BaseResultDTO
    {
        public UpdateRoomTypeDataResult Data { get; set; }
    }

    public class UpdateRoomTypeDataResult
    {
        public string Type { get; set; }
        public decimal ExtraPrice { get; set; }
    }
}
