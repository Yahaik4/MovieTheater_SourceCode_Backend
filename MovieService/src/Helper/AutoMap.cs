using AutoMapper;
using AutoMapper.Internal;
using Google.Protobuf.Collections;
using MovieGrpc;
using MovieService.DataTransferObject.ResultData;

namespace MovieService.Helper
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            //AllowNullCollections = true;
            //AllowNullDestinationValues = true;
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

            // Movie

            CreateMap<MovieGenreDataResult, MovieGenreGrpcReplyDataDTO>();
            CreateMap<MoviePersonDataResult, MoviePersonGrpcReplyDataDTO>();

            CreateMap<GetMoviesResultData, GetMoviesGrpcReplyDTO>();
            CreateMap<GetMoviesDataResult, GetMoviesGrpcReplyDataDTO>();

            CreateMap<CreateMovieResultData, CreateMovieGrpcReplyDTO>();
            CreateMap<CreateMovieDataResult, CreateMovieGrpcReplyDataDTO>();

            CreateMap<UpdateMovieResultData, UpdateMovieGrpcReplyDTO>();
            CreateMap<UpdateMovieDataResult, UpdateMovieGrpcReplyDataDTO>();

            CreateMap<DeleteMovieResultData, DeleteMovieGrpcReplyDTO>();
        }

    }
}
