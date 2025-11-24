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
            CreateMap<GetCustomersResultData, GetCustomersGrpcReplyDTO>()
                .ForMember(dest => dest.Customers, opt => opt.MapFrom(src => src.Data));

            CreateMap<CustomerWithProfileResultData, CustomerWithProfileGrpcDTO>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserInfoGrpcDTO
                {
                    UserId = src.UserId.ToString(),
                    FullName = src.FullName,
                    Email = src.Email,
                    Role = src.Role,
                    IsVerified = src.IsVerified
                }));

            // GetStaffsMapping
            CreateMap<GetStaffsResultData, GetStaffsGrpcReplyDTO>()
                .ForMember(dest => dest.Staffs, opt => opt.MapFrom(src => src.Data));

            CreateMap<StaffWithProfileResultData, StaffWithProfileGrpcDTO>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserInfoGrpcDTO
                {
                    UserId = src.UserId.ToString(),
                    FullName = src.FullName,
                    Email = src.Email,
                    Role = src.Role,
                    IsVerified = src.IsVerified
                }));

            CreateMap<DeleteUserResultData, DeleteUserGrpcReplyDTO>();

            CreateMap<UpdateCustomerResultData, UpdateCustomerGrpcReplyDTO>();
            CreateMap<UpdateStaffResultData, UpdateStaffGrpcReplyDTO>();
        }
        
    }
}
