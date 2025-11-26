namespace ApiGateway.DataTransferObject.ResultData
{
    public class CreateTransactionResultDTO : BaseResultDTO
    {
        public CreateTransactionDataResult Data { get; set; }
    }

    public class CreateTransactionDataResult
    {
        public string TransactionId { get; set; }           // Internal transaction ID (hoặc txnRef)
        public string? PaymentIntentId { get; set; }        // Stripe PaymentIntentId
        public string? ClientSecret { get; set; }           // Stripe ClientSecret
        public decimal Amount { get; set; }                 // Giá gốc VND
        public string Currency { get; set; }                // VNPay: "VND", Stripe: "usd"
        public string Status { get; set; }                  // Pending | Success | Failed
        public string Provider { get; set; }                // vnpay | stripe | momo
        public string? ProviderMeta { get; set; }           // JSON data nếu cần
        public string? PaymentUrl { get; set; }             // URL thanh toán (VNPay / Momo)
        public DateTime CreatedAt { get; set; }
    }
}
