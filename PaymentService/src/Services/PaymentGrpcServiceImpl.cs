using AutoMapper;
using Grpc.Core;
using PaymentGrpc;
using PaymentService.DataTransferObject.Parameter;
using PaymentService.DomainLogic;
using Shared.Contracts.Exceptions;

namespace PaymentService.Services
{
    public class PaymentGrpcServiceImpl : PaymentGrpcService.PaymentGrpcServiceBase
    {
        private readonly IMapper _mapper;
        private readonly CreateTransactionLogic _createTransactionLogic;
        private readonly HandleVnpayCallbackLogic _hanldeVnpayCallbackLogic;
        public PaymentGrpcServiceImpl(IMapper mapper, CreateTransactionLogic createTransactionLogic, HandleVnpayCallbackLogic hanldeVnpayCallbackLogic)

        {
            _mapper = mapper;
            _createTransactionLogic = createTransactionLogic;
            _hanldeVnpayCallbackLogic = hanldeVnpayCallbackLogic;
        }

        public override async Task<CreateTransactionGrpcReplyDTO> CreateTransaction(CreateTransactionGrpcRequestDTO request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.BookingId) ||
    !Guid.TryParse(request.BookingId, out var bookingId))
            {
                throw new ValidationException("Invalid or missing BookingId");
            }

            if (string.IsNullOrWhiteSpace(request.UserId) ||
                !Guid.TryParse(request.UserId, out var userId))
            {
                throw new ValidationException("Invalid or missing UserId");
            }

            var result = await _createTransactionLogic.Execute(new CreateTransactionParam
            {
                UserId = userId,
                BookingId = bookingId,
                PaymentGateway = request.PaymentGateway,
                ClientIp = request.ClientIp,
            });

            return _mapper.Map<CreateTransactionGrpcReplyDTO>(result);
        }

        public override async Task<HanldeVnpayCallbackGrpcReplyDTO> HanldeVnpayCallback(HanldeVnpayCallbackGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _hanldeVnpayCallbackLogic.Execute(new HandleVnpayCallbackParam
            {
                vnp_Amount = request.VnpAmount,
                vnp_BankCode = request.VnpBankCode,
                vnp_BankTranNo = request.VnpBankTranNo,
                vnp_CardType = request.VnpCardType,
                vnp_OrderInfo = request.VnpOrderInfo,
                vnp_PayDate = request.VnpPayDate,
                vnp_ResponseCode = request.VnpResponseCode,
                vnp_SecureHash = request.VnpSecureHash,
                vnp_TmnCode = request.VnpTmnCode,
                vnp_TransactionNo = request.VnpTransactionNo,
                vnp_TransactionStatus = request.VnpTransactionStatus,
                vnp_TxnRef = request.VnpTxnRef
            });

            return _mapper.Map<HanldeVnpayCallbackGrpcReplyDTO>(result);
        }
    }
}
