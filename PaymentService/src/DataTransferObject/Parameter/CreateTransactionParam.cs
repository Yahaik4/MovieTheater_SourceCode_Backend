using Shared.Contracts.Interfaces;

namespace PaymentService.DataTransferObject.Parameter
{
    public class CreateTransactionParam : IParam
    {
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public string PaymentGateway { get; set; }
        public string ClientIp { get; set; }
    }
}
