namespace ApiGateway.DataTransferObject.ResultData
{
    public class CreateTransactionResultDTO : BaseResultDTO
    {
        public CreateTransactionDataResult Data { get; set; }
    }

    public class CreateTransactionDataResult
    {
        public Guid TransactionId { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
