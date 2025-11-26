namespace ApiGateway.DataTransferObject.Parameter
{
    public class CreateTransactionRequestParam
    {
        public Guid BookingId { get; set; }
        public string PaymentGateway { get; set; }
    }
}
