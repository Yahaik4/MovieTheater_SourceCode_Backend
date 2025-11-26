using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src.Shared.Contracts.Messages
{
    public class PaymentStatusChangedMessage
    {
        public Guid TransactionId { get; set; }
        public Guid BookingId { get; set; }
        public string Provider { get; set; } // Momo / VnPay / Stripe
        public string Status { get; set; }   // Paid | Failed
        public decimal Amount { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
