namespace ApiGateway.DataTransferObject.Parameter
{
    public class GetPromotionsRequestParam
    {
        public Guid? Id { get; set; }
        public string? Code { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? DiscountType { get; set; }
        public bool? IsActive { get; set; }
    }
}
