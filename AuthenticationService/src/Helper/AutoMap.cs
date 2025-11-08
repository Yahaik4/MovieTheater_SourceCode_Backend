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

            // LogoutMapping
            CreateMap<LogoutResultData, LogoutGrpcReplyDTO>();

            // RegisterMapping
            CreateMap<RegisterResultData, RegisterGrpcReplyDTO>();

            CreateMap<ChangePasswordResultData, ChangePasswordGrpcReplyDTO>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));
            CreateMap<ChangePasswordDataResult, ChangePasswordGrpcReplyDataDTO>();
            
            CreateMap<ChangePasswordResultData, ConfirmChangePasswordGrpcReplyDTO>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));
            CreateMap<ChangePasswordDataResult, ConfirmChangePasswordGrpcReplyDataDTO>();
        }
    }
}
