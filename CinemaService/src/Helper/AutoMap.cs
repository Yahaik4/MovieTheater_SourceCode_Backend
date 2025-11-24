using AutoMapper;
using CinemaGrpc;
using CinemaService.DataTransferObject.ResultData;

namespace CinemaService.Helper
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

            CreateMap<GetAllSeatResultData, GetAllSeatsGrpcReplyDTO>();
            CreateMap<GetAllSeatDataResult, GetAllSeatsGrpcReplyDataDTO>();

            CreateMap<UpdateSeatsResultData, UpdateSeatsGrpcReplyDTO>();
            CreateMap<UpdateSeatsDataResult, UpdateSeatsGrpcReplyDataDTO>();

            CreateMap<GetShowtimesResultData, GetShowtimesGrpcReplyDTO>();
            CreateMap<GetShowtimesDataResult, GetShowtimesGrpcReplyDataDTO>();
            CreateMap<GetRoomTypeDataResult, GetRoomTypeGrpcReplyDataDTO>();
            CreateMap<ShowtimeDataResult, ShowtimeGrpcReplyDataDTO>();

            CreateMap<CreateShowtimeResultData, CreateShowtimeGrpcReplyDTO>();
            CreateMap<CreateShowtimeDataResult, CreateShowtimeGrpcReplyDataDTO>();

            CreateMap<UpdateShowtimeResultData, UpdateShowtimeGrpcReplyDTO>();
            CreateMap<UpdateShowtimeDataResult, UpdateShowtimeGrpcReplyDataDTO>();

            CreateMap<GetShowtimeSeatsResultData, GetShowtimeSeatsGrpcReplyDTO>();
            CreateMap<GetShowtimeSeatsDataResult, GetShowtimeSeatsGrpcReplyDataDTO>();

            CreateMap<GetBookingResultData, GetBookingGrpcReplyDTO>();
            CreateMap<GetBookingDataResult, GetBookingGrpcReplyDataDTO>();

            CreateMap<CreateBookingResultData, CreateBookingGrpcReplyDTO>();
            CreateMap<CreateBookingDataResult, CreateBookingGrpcReplyDataDTO>();
            CreateMap<BookingSeatsDataResult, CreateBookingSeatsGrpcReplyDataDTO>();

            CreateMap<UpdateBookingResultData, UpdateBookingGrpcReplyDTO>();
            CreateMap<UpdateBookingDataResult, UpdateBookingGrpcReplyDataDTO>();
            CreateMap<UpdateBookingSeatsDataResult, UpdateBookingSeatsGrpcReplyDataDTO>();

            CreateMap<GetAllFoodDrinkResultData, GetAllFoodDrinksGrpcReplyDTO>();
            CreateMap<GetAllFoodDrinkDataResult, GetAllFoodDrinksGrpcReplyDataDTO>();

            CreateMap<CreateFoodDrinkResultData, CreateFoodDrinkGrpcReplyDTO>();
            CreateMap<CreateFoodDrinkDataResult, CreateFoodDrinkGrpcReplyDataDTO>();

            CreateMap<UpdateFoodDrinkResultData, UpdateFoodDrinkGrpcReplyDTO>();
            CreateMap<UpdateFoodDrinkDataResult, UpdateFoodDrinkGrpcReplyDataDTO>();

            CreateMap<DeleteFoodDrinkResultData, DeleteFoodDrinkGrpcReplyDTO>();
            CreateMap<BookingFoodDrinkDataResult, CreateBookingFoodDrinkGrpcReplyDataDTO>();
        }
    }
}
