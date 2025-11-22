using Shared.Contracts.Interfaces;

namespace PaymentService.DataTransferObject.Parameter
{
    public class CreateTransactionParam : IParam
    {
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public string Currency { get; set; } = "usd";
        public string PaymentMethod { get; set; }
    }
}
