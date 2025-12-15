namespace ApiGateway.DataTransferObject.ResultData
{
    public class CreateHolidayResultDTO : BaseResultDTO
    {
        public CreateHolidayDataResult Data { get; set; }
    }

    public class CreateHolidayDataResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public decimal ExtraPrice { get; set; }
    }
}
