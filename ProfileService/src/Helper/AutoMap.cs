using AutoMapper;
using ProfileGrpc;
using src.DataTransferObject.ResultData;

namespace src.Helper
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
