namespace ApiGateway.DataTransferObject.ResultData
{
    public class GetAllRoomTypesResultDTO : BaseResultDTO
    {
        public List<GetAllRoomTypesDataResult> Data { get; set; }
    }

    public class GetAllRoomTypesDataResult
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public decimal ExtraPrice { get; set; }
    }
}
