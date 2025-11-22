using PaymentService.DataTransferObject.Parameter;
using PaymentService.DataTransferObject.ResultData;
using PaymentService.Infrastructure.Repositories.Interfaces;
using PaymentService.ServiceConnector.CinemaService;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using Stripe;

namespace PaymentService.DomainLogic
{
    public class CreateTransactionLogic : IDomainLogic<CreateTransactionParam, Task<CreateTransactionResultData>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly CinemaServiceConnector _cinemaServiceConnector;

        public CreateTransactionLogic(ITransactionRepository transactionRepository, CinemaServiceConnector cinemaServiceConnector)
        {
            _transactionRepository = transactionRepository;
            _cinemaServiceConnector = cinemaServiceConnector;
        }

        public async Task<CreateTransactionResultData> Execute(CreateTransactionParam param)
        {
            if (param == null) throw new ValidationException("Parameter is null");
            if (param.UserId == Guid.Empty) throw new ValidationException("UserId cannot be empty GUID");
            if (param.BookingId == Guid.Empty) throw new ValidationException("BookingId cannot be empty GUID");

            var currency = param.Currency.Trim().ToLowerInvariant();

            var booking = await _cinemaServiceConnector.GetBooking(param.BookingId);

            if(booking == null || !booking.Result)
            {
                throw new ValidationException("Booking not found");
            }

            if(booking.Data.Status != "pending" || booking.Data.UserId != param.UserId.ToString())
            {
                throw new ValidationException("Booking cannot be processed for payment");
            }

            decimal amount = decimal.Parse(booking.Data.TotalPrice);
            long stripeAmount;
            try
            {
                stripeAmount = ConvertToStripeAmount(amount, currency);
            }
            catch (Exception ex)
            {
                throw new ValidationException($"Unsupported currency or invalid amount: {ex.Message}");
            }

            try
            {
                var paymentIntentService = new PaymentIntentService();

                var createOptions = new PaymentIntentCreateOptions
                {
                    Amount = stripeAmount,
                    Currency = currency,
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

                var transaction = new PaymentService.Infrastructure.EF.Models.Transaction
                {
                    Id = Guid.NewGuid(),
                    BookingId = param.BookingId,
                    UserId = param.UserId,
                    PaymentIntentId = paymentIntent.Id,
                    Status = paymentIntent.Status,
                    Amount = amount,
                    Currency = currency,
                    PaymentMethod = paymentIntent.PaymentMethodTypes != null && paymentIntent.PaymentMethodTypes.Count > 0
                        ? paymentIntent.PaymentMethodTypes.First()
                        : null
                };

                await _transactionRepository.CreateTransaction(transaction);

                return new CreateTransactionResultData
                {
                    Result = true,
                    Message = "PaymentIntent created",
                    StatusCode = StatusCodeEnum.Created,
                    Data = new CreateTransactionDataResult
                    {
                        TransactionId = transaction.Id,
                        PaymentIntentId = paymentIntent.Id,
                        ClientSecret = paymentIntent.ClientSecret,
                        Amount = amount,
                        Currency = currency,
                        Status = paymentIntent.Status,
                        CreatedAt = transaction.CreatedAt
                    }
                };
            }
            catch (StripeException sx)
            {
                throw new Grpc.Core.RpcException(
                    new Grpc.Core.Status(Grpc.Core.StatusCode.FailedPrecondition, $"Stripe error: {sx.Message}")
                );
            }
            catch (Exception ex)
            {
                throw new Grpc.Core.RpcException(
                    new Grpc.Core.Status(Grpc.Core.StatusCode.Internal, $"Failed to create PaymentIntent: {ex.Message}")
                );
            }
        }

        private static long ConvertToStripeAmount(decimal amount, string currency)
        {
            // currencies with 0 decimal places
            var zeroDecimalCurrencies = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "jpy", "krw" // add any other zero-decimal currencies if needed
            };

            if (zeroDecimalCurrencies.Contains(currency))
            {
                // amount must be an integer for zero-decimal currencies
                if (decimal.Truncate(amount) != amount)
                    throw new ArgumentException($"Currency {currency} does not support fractional amounts.");
                return (long)amount;
            }

            // default: 2 decimal currencies
            // To avoid floating rounding issues, use MidpointRounding.ToEven or a safe rounding strategy
            var multiplied = Math.Round(amount * 100m, 0, MidpointRounding.AwayFromZero);
            if (multiplied < 0) throw new ArgumentException("Amount must be non-negative after conversion.");
            return (long)multiplied;
        }
    }
}
