using AutoMapper;
using Grpc.Core;
using PaymentGrpc;
using PaymentService.DataTransferObject.Parameter;
using PaymentService.DomainLogic;

namespace PaymentService.Services
{
    public class PaymentGrpcServiceImpl : PaymentGrpcService.PaymentGrpcServiceBase
    {
        private readonly IMapper _mapper;
        private readonly CreateTransactionLogic _createTransactionLogic;
        public PaymentGrpcServiceImpl(IMapper mapper, CreateTransactionLogic createTransactionLogic)

        {
            _mapper = mapper;
            _createTransactionLogic = createTransactionLogic;
        }

        public override async Task<CreateTransactionGrpcReplyDTO> CreateTransaction(CreateTransactionGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _createTransactionLogic.Execute(new CreateTransactionParam
            {
                UserId = Guid.Parse(request.UserId),
                BookingId = Guid.Parse(request.BookingId),
                PaymentMethod = request.PaymentMethod,
                Currency = request.Currency,
            });

            return _mapper.Map<CreateTransactionGrpcReplyDTO>(result);
        }
    }
}
