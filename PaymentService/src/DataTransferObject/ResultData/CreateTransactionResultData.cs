using PaymentService.Infrastructure.EF.Models;
using Shared.Contracts.ResultData;

namespace PaymentService.DataTransferObject.ResultData
{
    public class CreateTransactionResultData : BaseResultData
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
