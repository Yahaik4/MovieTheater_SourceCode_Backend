using AuthenticationGrpc;
using AutoMapper;
using src.DataTransferObject.ResultData;

namespace src.Helper
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

            CreateMap<LogoutResultData, LogoutGrpcReplyDTO>();
        }
    }
}
