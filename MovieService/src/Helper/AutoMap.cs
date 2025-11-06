using AutoMapper;
using AutoMapper.Internal;
using Google.Protobuf.Collections;
using MovieGrpc;
using src.DataTransferObject.ResultData;

namespace src.Helper
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;

            //GetGenresMapping
            CreateMap<GetGenresResultData, GetGenresGrpcReplyDTO>();
            CreateMap<GetGenresDataResult, GetGenresGrpcReplyDataDTO>();

            //GetGenresMapping
            CreateMap<CreateGenreResultData, CreateGenreGrpcReplyDTO>();
            CreateMap<CreateGenreDataResult, CreateGenreGrpcReplyDataDTO>();

            //GetGenresMapping
            CreateMap<UpdateGenreResultData, UpdateGenreGrpcReplyDTO>();
            CreateMap<UpdateGenreDataResult, UpdateGenreGrpcReplyDataDTO>();

            //GetGenresMapping
            CreateMap<DeleteGenreResultData, DeleteGenreGrpcReplyDTO>();

            //GetGenresMapping
            CreateMap<GetPersonsDataResult, GetPersonsGrpcReplyDataDTO>();
            CreateMap<GetPersonsResultData, GetPersonsGrpcReplyDTO>();

            //GetGenresMapping
            CreateMap<CreatePersonResultData, CreatePersonGrpcReplyDTO>();
            CreateMap<CreatePersonDataResult, CreatePersonGrpcReplyDataDTO>();

            //GetGenresMapping
            CreateMap<UpdatePersonResultData, UpdatePersonGrpcReplyDTO>();
            CreateMap<UpdatePersonDataResult, UpdatePersonGrpcReplyDataDTO>();

            //GetGenresMapping
            CreateMap<DeletePersonResultData, DeletePersonGrpcReplyDTO>();
        }

        //private static RepeatedField<GetPersonsGrpcReplyDataDTO> ConvertPersons(List<GetPersonsDataResult> data)
        //{
        //    var repeated = new RepeatedField<GetPersonsGrpcReplyDataDTO>();

        //    if (data != null)
        //    {
        //        foreach (var item in data)
        //        {
        //            repeated.Add(new GetPersonsGrpcReplyDataDTO
        //            {
        //                Id = item.Id.ToString(),
        //                FullName = item.FullName,
        //                Gender = item.Gender,
        //                BirthDate = item.BirthDate.HasValue
        //                    ? item.BirthDate.Value.ToString("yyyy-MM-dd")
        //                    : "",
        //                Nationality = item.Nationality,
        //                Bio = item.Bio,
        //                ImageUrl = item.ImageUrl
        //            });
        //        }
        //    }

        //    return repeated;
        //}
    }
}
