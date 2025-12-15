namespace ApiGateway.DataTransferObject.Parameter
{
    public class CreateHolidayRequestParam
    {
        public string Name { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public decimal ExtraPrice { get; set; }
    }
}
