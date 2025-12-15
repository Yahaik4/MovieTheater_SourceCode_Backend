namespace ApiGateway.DataTransferObject.ResultData
{
    public class UpdateHolidayResultDTO : BaseResultDTO
    {
        public UpdateHolidayDataResult Data { get; set; }
    }

    public class UpdateHolidayDataResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public decimal ExtraPrice { get; set; }
    }
}
