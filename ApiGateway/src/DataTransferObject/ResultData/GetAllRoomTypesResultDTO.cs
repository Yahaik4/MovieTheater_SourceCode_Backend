namespace src.DataTransferObject.ResultData
{
    public class GetAllRoomTypesResultDTO : BaseResultDTO
    {
        public List<GetAllRoomTypesDataResult> Data { get; set; }
    }

    public class GetAllRoomTypesDataResult
    {
        public string Type { get; set; }
        public decimal BasePrice { get; set; }
    }
}
