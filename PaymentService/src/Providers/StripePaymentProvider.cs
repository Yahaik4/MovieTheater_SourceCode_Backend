using PaymentService.DataTransferObject.Parameter;
using PaymentService.DataTransferObject.ResultData;
using PaymentService.ServiceConnector.CinemaService;
using Stripe;
using Shared.Contracts.Exceptions;

namespace PaymentService.Providers
{
    public class StripePaymentProvider : IPaymentProvider
    {
        private readonly CinemaServiceConnector _cinemaServiceConnector;

        public StripePaymentProvider(CinemaServiceConnector cinemaServiceConnector)
        {
            _cinemaServiceConnector = cinemaServiceConnector;
        }

        public async Task<CreateTransactionDataResult> CreatePaymentAsync(BookingDataParam param)
        {
            long stripeAmount = (long)(param.Amount * 100); // Stripe nhận cents

            var paymentIntentService = new PaymentIntentService();
            var createOptions = new PaymentIntentCreateOptions
            {
                Amount = stripeAmount,
                Currency = "usd",
                Metadata = new Dictionary<string, string>
                {
                    { "bookingId", param.BookingId.ToString() },
                    { "userId", param.UserId.ToString() }
                },
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true
                }
            };

            var paymentIntent = await paymentIntentService.CreateAsync(createOptions);

            return new CreateTransactionDataResult
            {
                TransactionId = paymentIntent.Id,
                PaymentIntentId = paymentIntent.Id,
                ClientSecret = paymentIntent.ClientSecret,
                Amount = param.Amount,
                Currency = "vnd",   // Stripe nhận USD
                Status = paymentIntent.Status,
                Provider = "stripe",
                ProviderMeta = null,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
