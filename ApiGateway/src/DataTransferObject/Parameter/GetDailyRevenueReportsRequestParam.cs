namespace ApiGateway.DataTransferObject.Parameter
{
    public class GetDailyRevenueReportsRequestParam
    {
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public Guid? CinemaId { get; set; }
    }
}
