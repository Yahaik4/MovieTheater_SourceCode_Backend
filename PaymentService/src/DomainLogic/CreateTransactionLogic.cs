using PaymentService.DataTransferObject.Parameter;
using PaymentService.DataTransferObject.ResultData;
using PaymentService.Infrastructure.Repositories.Interfaces;
using PaymentService.ServiceConnector.CinemaService;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using PaymentService.Providers;
using PaymentService.Infrastructure.EF.Models;

namespace PaymentService.DomainLogic
{
    public class CreateTransactionLogic : IDomainLogic<CreateTransactionParam, Task<CreateTransactionResultData>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly CinemaServiceConnector _cinemaServiceConnector;
        private readonly PaymentProviderFactory _paymentProviderFactory;

        public CreateTransactionLogic(
            ITransactionRepository transactionRepository,
            CinemaServiceConnector cinemaServiceConnector,
            PaymentProviderFactory paymentProviderFactory)
        {
            _transactionRepository = transactionRepository;
            _cinemaServiceConnector = cinemaServiceConnector;
            _paymentProviderFactory = paymentProviderFactory;
        }

        public async Task<CreateTransactionResultData> Execute(CreateTransactionParam param)
        {
            Console.WriteLine($"[DEBUG] Param: UserId={param.UserId}, BookingId={param.BookingId}, PaymentGateway={param.PaymentGateway}, ClientIp={param.ClientIp}");
            if (param == null) throw new ValidationException("Parameter is null");
            if (param.UserId == Guid.Empty) throw new ValidationException("UserId cannot be empty GUID");
            if (param.BookingId == Guid.Empty) throw new ValidationException("BookingId cannot be empty GUID");
            if (string.IsNullOrWhiteSpace(param.PaymentGateway))
                throw new ValidationException("PaymentGateway is required");

            // Lấy thông tin booking
            var booking = await _cinemaServiceConnector.GetBooking(param.BookingId);
            if (booking == null || !booking.Result)
                throw new ValidationException("Booking not found");
            if (booking.Data.Status != "pending")
                throw new ValidationException("Booking cannot be processed for payment");

            // Tổng tiền booking
            decimal amount = decimal.Parse(booking.Data.TotalPrice);

            // Lấy provider theo PaymentGateway
            var provider = _paymentProviderFactory.GetProvider(param.PaymentGateway);

            BookingDataParam bookingDataParam = new BookingDataParam
            {
                BookingId = param.BookingId,
                UserId = param.UserId,
                ClientIp = param.ClientIp,
                MovieId = Guid.Parse(booking.Data.MovieId),
                MovieName = booking.Data.MovieName,
                Amount = amount,
                StartTime = booking.Data.StartTime,
                EndTime = booking.Data.EndTime,
            };

            // Tạo transaction qua provider
            var transactionResult = await provider.CreatePaymentAsync(bookingDataParam);

            // Lưu vào database chung Transaction table
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                BookingId = param.BookingId,
                UserId = param.UserId,
                Amount = amount,
                Currency = transactionResult.Currency,
                Status = transactionResult.Status,
                PaymentGateway = param.PaymentGateway.ToLower(),
                TxnRef = transactionResult.TransactionId,
                PaymentGatewayTransactionNo = transactionResult.PaymentIntentId,
                ProviderMeta = transactionResult.ProviderMeta,
                CreatedAt = DateTime.UtcNow
            };

            await _transactionRepository.CreateTransaction(transaction);

            // Trả về dữ liệu chuẩn chung
            return new CreateTransactionResultData
            {
                Result = true,
                Message = "Transaction created",
                StatusCode = StatusCodeEnum.Created,
                Data = new CreateTransactionDataResult
                {
                    TransactionId = transaction.TxnRef,
                    PaymentIntentId = transaction.PaymentGatewayTransactionNo,
                    Amount = transaction.Amount,
                    Currency = transaction.Currency,
                    Status = transaction.Status,
                    PaymentUrl = transactionResult.PaymentUrl,
                    Provider = transaction.PaymentGateway,
                    ProviderMeta = transaction.ProviderMeta,
                    CreatedAt = transaction.CreatedAt
                }
            };
        }
    }
}
