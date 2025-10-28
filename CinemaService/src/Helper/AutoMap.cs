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

            // CreateRoomTypeMapping
            CreateMap<CreateRoomTypeResultData, CreateRoomTypeGrpcReplyDTO>();
            CreateMap<CreateRoomTypeDataResult, CreateRoomTypeGrpcReplyDataDTO>();

            //GetAllRoomTypeMapping
            CreateMap<GetAllRoomTypeResultData, GetAllRoomTypesGrpcReplyDTO>();
            CreateMap<GetAllRoomTypesDataResult, GetAllRoomTypesGrpcReplyDataDTO>();

            // UpdateRoomTypeMapping
            CreateMap<UpdateRoomTypeResultData, UpdateRoomTypeGrpcReplyDTO>();
            CreateMap<UpdateRoomTypeDataResult, UpdateRoomTypeGrpcReplyDataDTO>();

            // DeleteRoomTypeMapping
            CreateMap<DeleteRoomTypeResultData, DeleteRoomTypeGrpcReplyDTO>();

        }
    }
}
