using AutoMapper;
using PaymentGrpc;
using PaymentService.DataTransferObject.ResultData;

namespace PaymentService.Helper
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            CreateMap<CreateTransactionResultData, CreateTransactionGrpcReplyDTO>();
            CreateMap<CreateTransactionDataResult, CreateTransactionGrpcReplyDataDTO>();

            CreateMap<HandleVnpayCallbackResultData, HanldeVnpayCallbackGrpcReplyDTO>();
        }
    }
}
