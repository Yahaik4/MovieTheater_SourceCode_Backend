namespace ApiGateway.DataTransferObject.Parameter
{
    public class CreateTransactionRequestParam
    {
        public Guid BookingId { get; set; }
        public string Currency { get; set; } = "usd";
        public string PaymentMethod { get; set; }
    }
}
