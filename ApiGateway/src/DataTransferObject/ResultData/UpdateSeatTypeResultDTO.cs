namespace src.DataTransferObject.ResultData
{
    public class UpdateSeatTypeResultDTO : BaseResultDTO
    {
        public UpdateSeatTypeDataResult Data { get; set; }
    }

    public class UpdateSeatTypeDataResult
    {
        public string Type { get; set; }
        public decimal ExtraPrice { get; set; }
    }
}
