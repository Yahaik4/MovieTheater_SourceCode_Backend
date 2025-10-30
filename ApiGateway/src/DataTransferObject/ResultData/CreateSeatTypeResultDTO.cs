namespace src.DataTransferObject.ResultData
{
    public class CreateSeatTypeResultDTO : BaseResultDTO
    {
        public CreateSeatTypeDataResult Data { get; set; }
    }

    public class CreateSeatTypeDataResult
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public decimal ExtraPrice { get; set; }
        public string CreatedBy { get; set; }
    }
}
