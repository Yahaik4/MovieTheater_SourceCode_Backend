namespace ApiGateway.DataTransferObject.ResultData
{
    public class GetHolidaysResultDTO : BaseResultDTO
    {
        public List<GetHolidaysDataResult> Data { get; set; }
    }

    public class GetHolidaysDataResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public decimal ExtraPrice { get; set; }
    }
}
