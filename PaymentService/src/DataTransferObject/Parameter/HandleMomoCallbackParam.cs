using Shared.Contracts.Interfaces;

namespace PaymentService.DataTransferObject.Parameter
{
    public class HandleMomoCallbackParam : IParam
    {
        public string OrderId { get; set; }
        public string RequestId { get; set; }
        public long Amount { get; set; }
        public string Signature { get; set; }
        public int ErrorCode { get; set; }
    }
}
