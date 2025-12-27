namespace ApiGateway.DataTransferObject.ResultData
{
    public class CreateRoomTypeResultDTO : BaseResultDTO
    {
        public CreateRoomTypeDataResult Data { get; set; }
    }

    public class CreateRoomTypeDataResult
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public decimal ExtraPrice { get; set; }
        public string CreatedBy { get; set; }
    }
}
