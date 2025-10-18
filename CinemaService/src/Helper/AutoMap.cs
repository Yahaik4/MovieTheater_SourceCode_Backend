using CinemaGrpc;
using AutoMapper;
using src.DataTransferObject.ResultData;

namespace src.Helper
{
    public class AutoMap : Profile
    {
        public AutoMap() 
        {
            // CreateCinemaMapping
            CreateMap<CreateCinemaResultData, CreateCinemaGrpcReplyDTO>();
            CreateMap<CreateCinemaDataResult, CreateCinemaGrpcReplyDataDTO>();
        }
    }
}
