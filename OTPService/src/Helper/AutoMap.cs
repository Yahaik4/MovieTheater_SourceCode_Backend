using AutoMapper;
using OTPGrpc;
using OTPService.DataTransferObject.ResultData;

namespace OTPService.Helper
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            CreateMap<CreateOTPResultData, CreateOTPGrpcReplyDTO>();
            CreateMap<CreateOTPDataResult, CreateOTPGrpcReplyDataDTO>();

            CreateMap<VerifyOTPResultData, VerifyOTPGrpcReplyDTO>();
        }
    }
}
