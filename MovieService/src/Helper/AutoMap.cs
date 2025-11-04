using AutoMapper;
using MovieGrpc;
using src.DataTransferObject.ResultData;

namespace src.Helper
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            //GetGenresMapping
            CreateMap<GetGenresResultData, GetGenresGrpcReplyDTO>();
            CreateMap<GetGenresDataResult, GetGenresGrpcReplyDataDTO>();

            CreateMap<CreateGenreResultData, CreateGenreGrpcReplyDTO>();
            CreateMap<CreateGenreDataResult, CreateGenreGrpcReplyDataDTO>();

            CreateMap<UpdateGenreResultData, UpdateGenreGrpcReplyDTO>();
            CreateMap<UpdateGenreDataResult, UpdateGenreGrpcReplyDataDTO>();

            CreateMap<DeleteGenreResultData, DeleteGenreGrpcReplyDTO>();
        }
    }
}
