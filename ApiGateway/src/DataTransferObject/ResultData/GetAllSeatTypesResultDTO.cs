namespace src.DataTransferObject.ResultData
{
    public class GetAllSeatTypesResultDTO : BaseResultDTO
    {
        public List<GetAllSeatTypesDataResult> Data { get; set; }
    }

    public class GetAllSeatTypesDataResult
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public decimal ExtraPrice { get; set; }
    }
}
