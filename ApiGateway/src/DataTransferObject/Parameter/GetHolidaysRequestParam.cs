namespace ApiGateway.DataTransferObject.Parameter
{
    public class GetHolidaysRequestParam
    {
        public string? Name { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
