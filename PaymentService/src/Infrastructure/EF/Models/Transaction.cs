using Shared.Infrastructure;

namespace PaymentService.Infrastructure.EF.Models
{
    public class Transaction : BaseEntity
    {
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string PaymentGateway { get; set; } 
        public string? PaymentMethodType { get; set; }
        public string TxnRef { get; set; }
        public string? PaymentGatewayTransactionNo { get; set; }
        public string? ProviderMeta { get; set; }
    }

}