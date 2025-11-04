using AutoMapper;
using MovieGrpc;
using Shared.Contracts.ResultData;
using src.DataTransferObject.ResultData;
using System;

namespace src.Helper
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            //GetGenresMapping
            CreateMap<GetGenresResultData, GetGenresGrpcReplyDTO>();
            CreateMap<GetGenresDataResult, GetGenresGrpcReplyDataDTO>();
        }
    }
}
