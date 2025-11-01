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

            // GetAllSeatTypeMapping
            CreateMap<GetAllSeatTypeResultData, GetAllSeatTypesGrpcReplyDTO>();
            CreateMap<GetAllSeatTypeDataResult, GetAllSeatTypesGrpcReplyDataDTO>();

            // CreateSeatTypeMapping
            CreateMap<CreateSeatTypeResultData, CreateSeatTypeGrpcReplyDTO>();
            CreateMap<CreateSeatTypeDataResult, CreateSeatTypeGrpcReplyDataDTO>();

            // UpdateSeatTypeMapping
            CreateMap<UpdateSeatTypeResultData, UpdateSeatTypeGrpcReplyDTO>();
            CreateMap<UpdateSeatTypeDataResult, UpdateSeatTypeGrpcReplyDataDTO>();

            // DeleteSeatTypeMapping
            CreateMap<DeleteSeatTypeResultData, DeleteSeatTypeGrpcReplyDTO>();

            CreateMap<CreateRoomResultData, CreateRoomGrpcReplyDTO>();
            CreateMap<CreateRoomDataResult, CreateRoomGrpcReplyDataDTO>();
            CreateMap<CreateSeatDataResult, CreateSeatGrpcReplyDataDTO>();

            CreateMap<GetAllRoomResultData, GetAllRoomsGrpcReplyDTO>();
            CreateMap<GetAllRoomDataResult, GetAllRoomsGrpcReplyDataDTO>();

            CreateMap<UpdateRoomResultData, UpdateRoomGrpcReplyDTO>();
            CreateMap<UpdateRoomDataResult, UpdateRoomGrpcReplyDataDTO>();

            CreateMap<DeleteRoomResultData, DeleteRoomGrpcReplyDTO>();

        }
    }
}
