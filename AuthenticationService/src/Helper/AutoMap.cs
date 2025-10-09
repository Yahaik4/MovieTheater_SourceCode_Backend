using AuthenticationGrpc;
using AutoMapper;
using src.DataTransferObject.ResultData;

namespace src.Helper
{
    public class AutoMap : Profile
    {
        public AutoMap() 
        {
            CreateMap<LoginResultData, LoginGprcReplyDTO>();
            CreateMap<LoginDataResult, LoginGprcReplyDataDTO>();
        }
    }
}
