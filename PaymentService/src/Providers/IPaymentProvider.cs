using PaymentService.DataTransferObject.Parameter;
using PaymentService.DataTransferObject.ResultData;

namespace PaymentService.Providers
{
    public interface IPaymentProvider
    {
        Task<CreateTransactionDataResult> CreatePaymentAsync(BookingDataParam param);
    }
}
