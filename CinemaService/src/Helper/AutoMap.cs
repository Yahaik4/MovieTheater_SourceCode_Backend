using AutoMapper;
using CinemaGrpc;
using Shared.Contracts.ResultData;
using src.DataTransferObject.ResultData;
using System;

namespace src.Helper
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            //CreateMap<TimeOnly, string>().ConvertUsing(t => t.ToString("HH:mm"));

            //GetAllCinemaMapping
            CreateMap<GetAllCinemasResultData, GetAllCinemasGrpcReplyDTO>();
            CreateMap<GetAllCinemasDataResult, GetAllCinemasGrpcReplyDataDTO>();

            // CreateCinemaMapping
            CreateMap<CreateCinemaResultData, CreateCinemaGrpcReplyDTO>();
            CreateMap<CreateCinemaDataResult, CreateCinemaGrpcReplyDataDTO>();

            // UpdateCinemaMapping
            CreateMap<UpdateCinemaResultData, UpdateCinemaGrpcReplyDTO>();
            CreateMap<UpdateCinemaDataResult, UpdateCinemaGrpcReplyDataDTO>();

            // DeleteCinemaMapping
            CreateMap<DeleteCinemaResultData, DeleteCinemaGrpcReplyDTO>();
        }
    }
}
