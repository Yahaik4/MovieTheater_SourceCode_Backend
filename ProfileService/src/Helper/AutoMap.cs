using AutoMapper;
using ProfileGrpc;
using ProfileService.DataTransferObject.ResultData;

namespace ProfileService.Helper
{
    public class AutoMap : Profile
    {
        public AutoMap() 
        {
            // CreateMapping
            CreateMap<CreateProfileResultData, CreateProfileGrpcReplyDTO>();
        }
    }
}
