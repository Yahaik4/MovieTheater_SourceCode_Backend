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
        public decimal BasePrice { get; set; }
        public string CreatedBy { get; set; }
    }
}
