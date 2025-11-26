using PaymentService.DataTransferObject.Parameter;
using PaymentService.DataTransferObject.ResultData;
using PaymentService.Infrastructure.Repositories.Interfaces;
using PaymentService.Libraries;
using PaymentService.ServiceConnector;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.Shared.Contracts.Messages;

namespace PaymentService.DomainLogic
{
    public class HandleVnpayCallbackLogic : IDomainLogic<HandleVnpayCallbackParam, Task<HandleVnpayCallbackResultData>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly RabbitMqPublisher _publisher;
        private readonly IConfiguration _configuration;

        public HandleVnpayCallbackLogic(ITransactionRepository transactionRepository, RabbitMqPublisher publisher, IConfiguration configuration)
        {
            _transactionRepository = transactionRepository;
            _publisher = publisher;
            _configuration = configuration;
        }

        public async Task<HandleVnpayCallbackResultData> Execute(HandleVnpayCallbackParam param)
        {
            var vnpLib = new VnPayLibrary();

            // ⭐ 1. Validate signature
            //string hashSecret = _configuration["VnPay:HashSecret"];
            //bool isValidSignature = vnpLib.ValidateSignature(param.vnp_SecureHash, hashSecret);

            //if (!isValidSignature)
            //    throw new ValidationException("Invalid VNPay signature");

            // ⭐ 2. Find transaction by vnp_TxnRef
            var transaction = await _transactionRepository.GetTransactionTxnRef(param.vnp_TxnRef);

            if (transaction == null)
                throw new NotFoundException($"Transaction with TxnRef {param.vnp_TxnRef} not found");

            // ⭐ 3. Validate amount (VNPay trả về = số tiền * 100)
            if (!long.TryParse(param.vnp_Amount, out long vnpAmount))
                throw new ValidationException("Invalid VNPAY amount format");

            long expectedAmount = (long)transaction.Amount * 100;

            if (vnpAmount != expectedAmount)
                throw new ValidationException("Amount mismatch between system and VNPay");

            // ⭐ 4. Determine payment status
            bool isSuccess = param.vnp_ResponseCode == "00" &&
                             param.vnp_TransactionStatus == "00";

            string status = isSuccess ? "paid" : "failed";

            // ⭐ 5. Update DB
            transaction.Status = status;
            transaction.PaymentGatewayTransactionNo = param.vnp_TransactionNo;
            transaction.UpdatedAt = DateTime.UtcNow;

            await _transactionRepository.UpdateTransaction(transaction);

            // ⭐ 6. Publish message to RabbitMQ
            var message = new PaymentStatusChangedMessage
            {
                TransactionId = transaction.Id,
                BookingId = transaction.BookingId,
                Provider = "VNPAY",
                Status = status,
                Amount = transaction.Amount,
                UpdatedAt = DateTime.UtcNow
            };

            _publisher.PublishPaymentStatus(message);

            // ⭐ 7. Return response
            return new HandleVnpayCallbackResultData
            {
                Result = true,
                Message = "Update Status Transsaction Successfully",
                StatusCode = StatusCodeEnum.Success
            };
        }
    }
}
