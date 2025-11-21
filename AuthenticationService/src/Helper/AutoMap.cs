using AuthenticationGrpc;
using AuthenticationService.DataTransferObject.ResultData;
using AutoMapper;

namespace AuthenticationService.Helper
{
    public class AutoMap : Profile
    {
        public AutoMap() 
        {
            // LoginMapping
            CreateMap<LoginResultData, LoginGrpcReplyDTO>();
            CreateMap<LoginDataResult, LoginGrpcReplyDataDTO>();

            // RefreshTokenMapping
            CreateMap<RefreshTokenResultData, RefreshTokenGrpcReplyDTO>();
            CreateMap<RefreshTokenDataResult, RefreshTokenGrpcReplyDataDTO>();

            // LogoutMapping
            CreateMap<LogoutResultData, LogoutGrpcReplyDTO>();

            // RegisterMapping
            CreateMap<RegisterResultData, RegisterGrpcReplyDTO>();
            CreateMap<RegisterDataResult, RegisterGrpcReplyDataDTO>();

            // VerifyAccountMapping
            CreateMap<VerifyAccountResultData, VerifyAccountGrpcReplyDTO>();
        }
    }
}
