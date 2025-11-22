using Shared.Infrastructure;

namespace PaymentService.Infrastructure.EF.Models
{
    public class Transaction : BaseEntity
    {
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public string PaymentIntentId { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } 
        public string PaymentMethod { get; set; }
    }
}